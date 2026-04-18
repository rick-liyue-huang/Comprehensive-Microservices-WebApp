# UsersMicroservice

用户微服务 — 隶属于 `Comprehensive-Microservices-WebApp` 解决方案，负责用户注册与登录。基于 **ASP.NET Core (net10.0)** + **Clean Architecture** 构建，使用 **Dapper + PostgreSQL** 做持久化，集成 **AutoMapper**、**FluentValidation**、**Swagger**。

> 项目级总览见上层 [../CLAUDE.md](../CLAUDE.md)。本文件仅描述 UsersMicroservice 自身。

---

## 目录结构

```
UsersMicroservice/
├── UsersMicroservice.API/                    # 表示层：Controllers、Middleware、Program.cs
│   ├── Controllers/
│   │   └── AuthController.cs                 # POST api/auth/register, api/auth/login
│   ├── Middlewares/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Program.cs                            # Pipeline 装配 + DI 注册
│   ├── appsettings.json                      # 含 ConnectionStrings、AutoMapper licenseKey
│   └── UsersMicroservice.API.csproj          # → Core, Infrastructure
│
├── UsersMicroservice.Core/                   # 应用层：DTO、Service、契约、映射、校验
│   ├── Dtos/
│   │   ├── LoginRequest.cs                   # record (Email, Password)
│   │   ├── RegisterRequest.cs                # record (Email, Password, PersonName, Gender)
│   │   ├── AuthenticationResponse.cs         # record (UserId, Email, PersonName, Gender, Token, Success)
│   │   └── GenderOptions.cs                  # enum: Male=1, Female, Others
│   ├── Mappers/
│   │   ├── ApplicationUserMappingProfile.cs  # ApplicationUser → AuthenticationResponse
│   │   └── RegisterRequestMappingProfile.cs  # RegisterRequest → ApplicationUser
│   ├── Validators/
│   │   ├── LoginRequestValidator.cs          # FluentValidation 规则
│   │   └── RegisterRequestValidator.cs       # FluentValidation 规则
│   ├── ServiceContracts/IUsersService.cs
│   ├── Services/UsersService.cs
│   ├── RepositoryContracts/IUsersRepository.cs
│   ├── DependencyInjection.cs                # AddCore()
│   └── UsersMicroservice.Core.csproj         # → Domain
│
├── UsersMicroservice.Domain/                 # 领域层：实体
│   ├── Entities/ApplicationUser.cs           # UserId, Email, Password, PersonName, Gender
│   └── UsersMicroservice.Domain.csproj       # 无依赖
│
└── UsersMicroservice.Infrastructure/         # 基础设施层：DbContext + Repository
    ├── DbContext/
    │   └── DapperDbContext.cs                # 包装 NpgsqlConnection
    ├── Repositories/
    │   └── UsersRepository.cs                # Dapper 实现（INSERT / SELECT users）
    ├── DependencyInjection.cs                # AddInfrastructure()
    └── UsersMicroservice.Infrastructure.csproj  # → Core
```

---

## 分层与依赖方向

```
API ──▶ Core ──▶ Domain
 │                ▲
 └──▶ Infrastructure ──┘
```

- **Domain**：纯实体，零依赖。
- **Core**：定义 `IUsersService` / `IUsersRepository`，实现业务逻辑（`UsersService`）、AutoMapper Profiles、FluentValidation Validators。
- **Infrastructure**：实现 `IUsersRepository`（Dapper + Npgsql），封装 `DapperDbContext`；引用 Core。
- **API**：组合根；同时引用 Core 与 Infrastructure，通过 `AddCore()` / `AddInfrastructure()` 扩展方法注册 DI。

DI 生命周期：`IUsersService`、`IUsersRepository`、`DapperDbContext` 均为 **Scoped**。

---

## 请求 / 响应流

```
HTTP → ExceptionHandlingMiddleware
     → Routing → Swagger → CORS → Authentication → Authorization
     → AuthController
        → FluentValidation 自动校验 DTO
        → IUsersService (UsersService)
            → AutoMapper（RegisterRequest → ApplicationUser / ApplicationUser → AuthenticationResponse）
            → IUsersRepository (UsersRepository)
                → DapperDbContext.DbConnection (Npgsql) → PostgreSQL
```

`Program.cs` 当前 pipeline（[Program.cs](UsersMicroservice.API/Program.cs)）：
1. `UseExceptionHandlingMiddleware` — 捕获全部异常，返回 500 + `{ Message, Type }` JSON
2. `UseRouting`
3. `UseSwagger` + `UseSwaggerUI`
4. `UseCors` — ⚠ 调用未指定策略名（未传入 `"AllowAll"`），policy 不会生效
5. `UseAuthentication` / `UseAuthorization` — ⚠ 已启用，但未注册任何 scheme（JWT 待接入）
6. `MapControllers`

> ⚠ `Program.cs` 末尾出现两次 `app.Run()`（第二次不可达），属小型残留 bug。

---

## 注册的服务（DI）

**API 层（`Program.cs`）**
- `AddInfrastructure()` / `AddCore()`
- `AddControllers()` + `JsonStringEnumConverter`（枚举以字符串序列化）
- `AddAutoMapper` — 扫描 `ApplicationUserMappingProfile.Assembly`，读取 `AutoMapper:licenseKey`
- `AddFluentValidationAutoValidation()`
- `AddEndpointsApiExplorer()` + `AddSwaggerGen()`
- `AddCors("AllowAll")` — 允许任意 Origin/Header/Method

**Core (`AddCore`)**
- `IUsersService → UsersService` (Scoped)
- `AddValidatorsFromAssemblyContaining<LoginRequestValidator>()`

**Infrastructure (`AddInfrastructure`)**
- `IUsersRepository → UsersRepository` (Scoped)
- `DapperDbContext` (Scoped)

---

## API 端点

| Method | 路由 | 说明 | 成功 | 失败 |
|---|---|---|---|---|
| POST | `/api/auth/register` | 注册新用户 | 200 + `AuthenticationResponse` | 400 + `AuthenticationResponse` |
| POST | `/api/auth/login`    | 登录          | 200 + `AuthenticationResponse` | 401 + `AuthenticationResponse` |

`AuthenticationResponse.Token` 当前硬编码字符串 `"token"`，**JWT 生成尚未实现**。

### 校验规则（FluentValidation）
- **RegisterRequest**：Email 必填且合法邮箱；Password 必填、长度 ≥ 6；PersonName 必填；Gender 必须在枚举范围内。
- **LoginRequest**：Email 必填且合法邮箱；Password 必填、长度 ≥ 6。

---

## 持久化

- **数据库**：PostgreSQL（独立库 `ecommerce_user_db`）
- **访问方式**：Dapper + Npgsql（非 EF Core）
- **连接字符串**（`appsettings.json`）：
  ```
  Host=localhost;Port=5436;Database=ecommerce_user_db;Username=postgres;Password=password
  ```
- **表**：`users(userid, email, password, personname, gender)` — 由仓储 SQL 推断，**表结构 / 迁移脚本未纳入仓库**，需手动建表。

### 仓储 SQL（[UsersRepository.cs](UsersMicroservice.Infrastructure/Repositories/UsersRepository.cs)）
```sql
-- AddUser
INSERT INTO Users (userid, email, password, personname, gender)
VALUES (@UserId, @Email, @Password, @PersonName, @Gender);

-- GetUserByEmailAndPassword
SELECT * FROM users WHERE email = @Email AND password = @Password;
```

---

## NuGet 依赖

| 项目 | 包 | 版本 |
|---|---|---|
| API | AutoMapper | 16.1.1 |
| API | FluentValidation.AspNetCore | 11.3.1 |
| API | Swashbuckle.AspNetCore | 10.1.7 |
| Core | AutoMapper | 16.1.1 |
| Core | FluentValidation.AspNetCore | 11.3.1 |
| Core | Microsoft.Extensions.DependencyInjection.Abstractions | 10.0.5 |
| Infrastructure | Dapper | 2.1.72 |
| Infrastructure | Npgsql | 10.0.2 |
| Infrastructure | Microsoft.Extensions.Configuration.Abstractions | 11.0.0-preview.2.26159.112 |
| Infrastructure | Microsoft.Extensions.DependencyInjection.Abstractions | 10.0.5 |

---

## 当前状态与待办

已完成
- [x] Clean Architecture 四层项目骨架
- [x] DI 扩展方法 `AddCore()` / `AddInfrastructure()`
- [x] 全局异常处理中间件
- [x] `AuthController` 注册/登录端点
- [x] AutoMapper 映射（DTO ↔ Entity）
- [x] FluentValidation 自动校验
- [x] Swagger / OpenAPI 启用
- [x] CORS 策略定义（"AllowAll"）
- [x] 枚举字符串序列化（`JsonStringEnumConverter`）
- [x] Dapper + Npgsql 持久化（`DapperDbContext` + `UsersRepository`）
- [x] `RegisterRequest.Email` 拼写修正

待办 / 已知问题
- [ ] **JWT 生成 & 校验**：`Token = "token"` 硬编码；`UseAuthentication` 未注册 scheme
- [ ] **密码哈希**：仍以明文存储与比对（[UsersService.Login](UsersMicroservice.Core/Services/UsersService.cs)、[UsersRepository](UsersMicroservice.Infrastructure/Repositories/UsersRepository.cs)）
- [ ] **CORS 未生效**：`app.UseCors()` 未传入策略名 `"AllowAll"`
- [ ] **`app.Run()` 调用两次**（[Program.cs:60-61](UsersMicroservice.API/Program.cs#L60-L61)）
- [ ] **`UsersRepository.AddUser` 签名不一致**：实现 `Task<ApplicationUser>`，接口 `Task<ApplicationUser?>`；实现体 `return ... ? user : null`，非空注解与运行时行为冲突
- [ ] **数据库迁移 / 建表脚本缺失**：`users` 表需手动创建
- [ ] **配置秘钥硬编码**：`AutoMapper:licenseKey`、PostgreSQL 密码写在 `appsettings.json`，应迁移到 user-secrets / 环境变量
- [ ] **`UsersController` 已移除**：原占位 GET 已不存在，如需用户列表 / 查询端点需重新实现
- [ ] **DTO 仍全为 nullable**：业务层已由 FluentValidation 保障，但类型级依旧宽松
- [ ] **单元 / 集成测试** 项目缺失

---

## 技术栈

- .NET 10.0（预览 / 早期版本，注意 SDK 兼容）
- ASP.NET Core Web API
- Dapper 2.1.72 + Npgsql 10.0.2（PostgreSQL）
- AutoMapper 16.1.1（Profile 模式，需 license key）
- FluentValidation.AspNetCore 11.3.1
- Swashbuckle.AspNetCore 10.1.7

尚未引入：EF Core、JWT Bearer、密码哈希库（如 `BCrypt.Net-Next`）、测试框架。

---

## 常用命令

在 `UsersMicroservice/` 目录下：

```bash
# 还原 / 构建 / 运行 API
dotnet restore
dotnet build
dotnet run --project UsersMicroservice.API

# 仅构建单个项目
dotnet build UsersMicroservice.Core/UsersMicroservice.Core.csproj

# Swagger UI（默认开发地址）
# https://localhost:<port>/swagger
```

启动前请确保 PostgreSQL 监听在 `localhost:5436` 且 `ecommerce_user_db` 已存在并已建 `users` 表。

---

## 开发约定

- **依赖方向不可逆**：API/Infrastructure → Core → Domain，禁止反向引用。
- **新增业务能力流程**：
  1. 在 `Domain/Entities` 增/改实体
  2. 在 `Core/RepositoryContracts` 添加仓储契约
  3. 在 `Core/ServiceContracts` + `Core/Services` 添加服务契约与实现
  4. 在 `Core/Mappers` 增加 AutoMapper Profile（如涉及 DTO ↔ Entity）
  5. 在 `Core/Validators` 增加 FluentValidation Validator（如有入参 DTO）
  6. 在 `Infrastructure/Repositories` 用 Dapper 实现仓储 SQL
  7. 确认 `Core/DependencyInjection.cs` 与 `Infrastructure/DependencyInjection.cs` 注册到位（Validators 会被程序集扫描自动注册）
  8. 在 `API/Controllers` 暴露端点
- **DTO 用 `record`**，实体用 `class`。
- **异常处理**统一交给 `ExceptionHandlingMiddleware`，Controller 内不要 try/catch 业务异常。
- **Controller 不得直接依赖 Repository**，必须经由 Service 层。
- **枚举序列化**使用字符串（全局 `JsonStringEnumConverter` 已注册），DTO 无需单独标注。

# UsersMicroservice

用户微服务 — 隶属于 `Comprehensive-Microservices-WebApp` 解决方案，负责用户注册、登录与认证。基于 **ASP.NET Core (net10.0)** + **Clean Architecture** 构建。

> 项目级总览见上层 [../CLAUDE.md](../CLAUDE.md)。本文件仅描述 UsersMicroservice 自身。

---

## 目录结构

```
UsersMicroservice/
├── UsersMicroservice.API/              # 表示层：Controllers、Middleware、Program.cs
│   ├── Controllers/
│   │   ├── AuthController.cs           # POST api/auth/register, api/auth/login
│   │   └── UsersController.cs          # GET  api/users (占位)
│   ├── Middlewares/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Program.cs                      # Pipeline 装配 + DI 注册
│   ├── appsettings.json
│   └── UsersMicroservice.API.csproj    # → Core, Infrastructure
│
├── UsersMicroservice.Core/             # 应用层：DTO、Service、契约
│   ├── Dtos/
│   │   ├── LoginRequest.cs             # record (Email, Password)
│   │   ├── RegisterRequest.cs          # record (Eamil*, Password, PersonName, Gender)
│   │   ├── AuthenticationResponse.cs   # record (UserId, Email, PersonName, Gender, Token, Success)
│   │   └── GenderOptions.cs            # enum: Male/Female/Others
│   ├── ServiceContracts/IUsersService.cs
│   ├── Services/UsersService.cs
│   ├── RepositoryContracts/IUsersRepository.cs
│   ├── DependencyInjection.cs          # AddCore()
│   └── UsersMicroservice.Core.csproj   # → Domain
│
├── UsersMicroservice.Domain/           # 领域层：实体
│   ├── Entities/ApplicationUser.cs     # UserId, Email, Password, PersonName, Gender
│   └── UsersMicroservice.Domain.csproj # 无依赖
│
└── UsersMicroservice.Infrastructure/   # 基础设施层：Repository 实现
    ├── Repositories/UsersRepository.cs # ⚠ 当前为内存桩实现，无 DbContext
    ├── DependencyInjection.cs          # AddInfrastructure()
    └── UsersMicroservice.Infrastructure.csproj  # → Core
```

\* `RegisterRequest.Eamil` 为现有源码中的拼写错误（应为 `Email`），尚未修复。

---

## 分层与依赖方向

```
API ──▶ Core ──▶ Domain
 │                ▲
 └──▶ Infrastructure ──┘
```

- **Domain**：纯实体，零依赖。
- **Core**：定义 `IUsersService` / `IUsersRepository`，实现业务逻辑（`UsersService`），引用 Domain。
- **Infrastructure**：实现 `IUsersRepository`，引用 Core（以使用契约和 Domain 实体）。
- **API**：组合根；同时引用 Core 与 Infrastructure，通过两个 `AddCore()` / `AddInfrastructure()` 扩展方法注册 DI。

依赖注入生命周期：`IUsersService` 与 `IUsersRepository` 均为 **Scoped**。

---

## 请求 / 响应流

```
HTTP → ExceptionHandlingMiddleware
     → Routing → Authentication → Authorization
     → AuthController
        → IUsersService (UsersService)
            → IUsersRepository (UsersRepository)
                → [TODO] DbContext / 持久化
```

`Program.cs` 当前 pipeline：
1. `UseExceptionHandlingMiddleware` — 捕获全部异常，记录日志，返回 500 + `{ Message, Type }` JSON
2. `UseRouting`
3. `UseAuthentication` / `UseAuthorization` — ⚠ 已启用，但尚未配置任何认证方案（JWT 等待接入）
4. `MapControllers`

---

## API 端点

| Method | 路由 | 说明 | 成功 | 失败 |
|---|---|---|---|---|
| POST | `/api/auth/register` | 注册新用户 | 200 + `AuthenticationResponse` | 400 |
| POST | `/api/auth/login` | 登录 | 200 + `AuthenticationResponse` | 401 |
| GET  | `/api/users` | 占位，返回 200 空 | — | — |

`AuthenticationResponse.Token` 当前硬编码字符串 `"token"`，**JWT 生成尚未实现**。

---

## 当前状态与待办（基于源码事实）

已完成
- [x] Clean Architecture 四层项目骨架
- [x] DI 扩展方法 `AddCore()` / `AddInfrastructure()`
- [x] 全局异常处理中间件
- [x] Auth/Users Controller 雏形
- [x] `UsersService` 注册/登录业务流

待办（代码中明确缺失）
- [ ] **持久化层**：`UsersRepository` 仍为内存桩实现，需引入 `DbContext`（EF Core）+ 数据库（独立 Users DB）
- [ ] **JWT 生成 & 校验**：`Token = "token"`；`UseAuthentication` 未注册 scheme
- [ ] **密码哈希**：当前明文存储/比对（`GetUserByEmailAndPassword`）
- [ ] **Swagger / OpenAPI**：`Program.cs` 注释提及但尚未启用
- [ ] **修正拼写**：`RegisterRequest.Eamil` → `Email`（同步更新 `UsersService.Register` 的引用）
- [ ] **`UsersRepository.AddUser` 返回类型**：实现签名为 `Task<ApplicationUser>`，但接口为 `Task<ApplicationUser?>`
- [ ] **输入校验**：DTO 全部为 nullable，无 `[Required]` / FluentValidation
- [ ] **`UsersController`** 实际功能（查询、更新等）
- [ ] **配置项**：`appsettings.json` 仅含日志配置，无连接字符串、JWT 设置
- [ ] **单元/集成测试** 项目缺失

---

## 技术栈

- .NET 10.0（`net10.0`，预览/早期版本，注意 SDK 兼容）
- ASP.NET Core Web API
- `Microsoft.Extensions.DependencyInjection.Abstractions` 10.0.5
- 尚未引入：EF Core、JWT Bearer、Swashbuckle、FluentValidation、AutoMapper 等

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
```

---

## 开发约定

- **依赖方向不可逆**：API/Infrastructure → Core → Domain，禁止反向引用。
- **新增业务能力流程**：
  1. 在 `Domain/Entities` 增/改实体
  2. 在 `Core/RepositoryContracts` 添加仓储契约
  3. 在 `Core/ServiceContracts` + `Core/Services` 添加服务契约与实现
  4. 在 `Infrastructure/Repositories` 实现仓储
  5. 在 `Core/DependencyInjection.cs` 与 `Infrastructure/DependencyInjection.cs` 注册
  6. 在 `API/Controllers` 暴露端点
- **DTO 用 `record`**，实体用 `class`。
- **异常处理**统一交给 `ExceptionHandlingMiddleware`，Controller 内不要 try/catch 业务异常。
- **不要在 Controller 中直接依赖 Repository**，必须经由 Service 层。

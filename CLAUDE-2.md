# Comprehensive-Microservices-WebApp

> ASP.NET Core 微服务项目 — 将单体架构拆分为多个独立微服务
> 参考课程：ASP.NET Core Microservices（Web Academy by Harsha Vardhan）

---

## 项目目标

将传统单体电商应用（UI → Services/Business Logic → Data Access → 单一数据库）
重构为四个独立微服务，每个服务拥有独立的业务逻辑、数据访问层和数据库。

---

## 微服务一览

| 微服务 | 职责 | 数据库 |
|---|---|---|
| `Products.Microservice` | 商品目录、库存管理 | Products DB |
| `Users.Microservice` | 用户注册、认证、资料管理 | Users DB |
| `Orders.Microservice` | 订单创建、状态追踪、历史记录 | Orders DB |
| `Payments.Microservice` | 支付事务、退款、支付方式 | Payments DB |

---

## 项目结构

```
Comprehensive-Microservices-WebApp/
├── CLAUDE.md
├── .claudeignore
├── docker-compose.yml                  # 待添加
│
├── Products.Microservice/
│   ├── Products.UI/                    # Controllers, Program.cs, Swagger
│   ├── Products.Core/                  # DTOs, Service Interfaces & Implementations
│   ├── Products.Domain/                # Entities, Repository Interfaces
│   └── Products.Infrastructure/        # DbContext, Repository Implementations, EF Migrations
│
├── Users.Microservice/
│   ├── Users.UI/
│   ├── Users.Core/
│   ├── Users.Domain/
│   └── Users.Infrastructure/
│
├── Orders.Microservice/
│   ├── Orders.UI/
│   ├── Orders.Core/
│   ├── Orders.Domain/
│   └── Orders.Infrastructure/
│
└── Payments.Microservice/
    ├── Payments.UI/
    ├── Payments.Core/
    ├── Payments.Domain/
    └── Payments.Infrastructure/
```

---

## 架构模式：Clean Architecture（四层）

每个微服务内部采用 Clean Architecture，层间依赖方向严格如下：

```
UI → Core → Domain ← Infrastructure
```

### 各层职责

**Domain 层**（最内层，无任何外部依赖）
- Entities：业务实体类（如 `Product`, `Order`）
- Repository Interfaces：`IProductRepository` 等接口定义

**Core 层**（业务逻辑层）
- DTOs：数据传输对象（`ProductDto`, `CreateProductRequest`）
- Service Interfaces：`IProductService`
- Service Implementations：调用 Repository Interface，处理业务规则

**Infrastructure 层**（数据访问实现）
- DbContext：Entity Framework Core 数据库上下文
- Repository Implementations：实现 Domain 层定义的接口
- EF Migrations：数据库迁移文件

**UI 层**（最外层，面向客户端）
- Controllers：API 端点，只调用 Core 层 Service
- `Program.cs`：服务注册（`builder.Services`）+ 中间件管道（`app.Use*`）
- Swagger 配置：API 文档

---

## 技术栈

| 类别 | 技术 |
|---|---|
| 框架 | ASP.NET Core (.NET 8+) |
| 架构模式 | Clean Architecture + Microservices |
| ORM | Entity Framework Core |
| 数据库 | SQL Server / PostgreSQL（各服务独立） |
| API 文档 | Swagger / OpenAPI |
| 同步通信 | HTTP/REST、gRPC（计划） |
| 异步通信 | RabbitMQ / Kafka（计划） |
| API 网关 | Ocelot / YARP（计划） |
| 容器化 | Docker + Docker Compose（计划） |
| 编排 | Kubernetes（计划） |
| 认证授权 | OAuth2 + JWT |
| 弹性库 | Polly（Retry、Circuit Breaker、Timeout） |
| 日志 | Serilog + 结构化日志 |
| 分布式追踪 | OpenTelemetry → Jaeger / Zipkin（计划） |
| 监控 | Prometheus + Grafana（计划） |
| CI/CD | GitHub Actions / Azure DevOps（计划） |

---

## 核心设计原则

### 1. 单一职责（SRP）
每个微服务只负责一个业务子域。`Products.Microservice` 只管商品，不碰订单逻辑。
Clean Architecture 各层也遵循 SRP：Controller 不写业务逻辑，Service 不写 SQL。

### 2. 松耦合（Loose Coupling）
服务之间通过 API 或消息队列交互，不直接引用对方的代码或数据库。
更新 `Orders.Microservice` 不应影响 `Payments.Microservice`。

### 3. 数据库隔离（Database Per Service）
每个服务拥有独立数据库，严禁跨服务直接 JOIN 数据表。
跨服务数据通过 API 调用或事件（Event-Driven）同步。

### 4. API 优先设计（API-First）
先定义 API 契约（Swagger/OpenAPI），再编写实现。
其他服务依赖 API 文档进行集成，而非依赖具体实现。

### 5. 故障隔离（Fault Isolation）
单个服务故障不应导致整个系统崩溃。
`Payments.Microservice` 故障时，`Orders.Microservice` 应能继续接单并排队等待。
使用 Polly 实现 Retry、Circuit Breaker、Fallback 保护上游服务。

### 6. 独立可扩展（Independent Scalability）
订单高峰期只扩容 `Orders.Microservice`，其他服务不受影响。
每个服务可根据自身负载独立调整实例数量。

### 7. 自治（Autonomy）
各服务团队可以独立选择技术栈、独立发布版本。
`Products.Microservice` 升级 .NET 9 时，其他服务不需要同步升级。

---

## 服务间通信策略

### 同步通信（实时请求-响应）
适用于需要立即获取结果的场景。

```csharp
// 使用 IHttpClientFactory，不要直接 new HttpClient
services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    client.BaseAddress = new Uri(configuration["Services:Inventory"]);
});
```

- **HTTP/REST**：标准场景，使用 `HttpClient` + `IHttpClientFactory`
- **gRPC**：低延迟、高吞吐场景（计划）

### 异步通信（事件驱动，解耦）
适用于不需要立即响应、允许最终一致的场景。

```
Orders.Microservice  ──publish──▶  RabbitMQ  ──subscribe──▶  Payments.Microservice
                                              ──subscribe──▶  Notification.Microservice
```

- **RabbitMQ / Kafka**：发布/订阅模式
- 典型场景：订单创建后发布事件，支付服务和通知服务各自订阅处理

---

## 弹性与容错（Polly）

所有对外 HTTP 调用必须包含弹性策略，从项目第一天开始配置。

```csharp
services.AddHttpClient<IProductService, ProductService>()
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))))
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

| 模式 | 作用 | 配置建议 |
|---|---|---|
| Retry + Exponential Backoff | 瞬时失败自动重试 | 最多 3 次，间隔指数增长 |
| Circuit Breaker（熔断器） | 连续失败后断开保护下游 | 5次失败触发，30秒后半开 |
| Timeout | 防止慢服务耗尽线程池 | 每个外部调用设 5-10 秒超时 |
| Fallback（降级） | 服务不可用时返回兜底数据 | 返回缓存数据或空集合 |

---

## 可观测性（Observability）

### 结构化日志（Serilog）

```csharp
// Program.cs
builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .Enrich.WithCorrelationId()   // 跨服务追踪关键
       .WriteTo.Console(new JsonFormatter()));
```

- 每条日志必须携带 `CorrelationId`，用于跨服务串联同一请求的所有日志
- 日志格式：JSON，便于 ELK Stack 等系统集中采集

### 分布式追踪（OpenTelemetry）

```csharp
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddJaegerExporter());
```

- 为每个跨服务请求生成唯一 `TraceId`
- 在 Jaeger / Zipkin 中查看完整的请求链路瀑布图

### 健康检查

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>()
    .AddUrlGroup(new Uri("http://dependency-service/health"), "dependency");

app.MapHealthChecks("/health");
```

---

## API 版本控制

```csharp
// 安装：dotnet add package Asp.Versioning.Mvc
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Controller 标注
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase { }
```

**版本规范：**
- URL 路径版本：`/api/v1/products` 和 `/api/v2/products` 同时存在
- 只增加字段，不删除/重命名已有字段（向后兼容原则）
- 破坏性变更（Breaking Change）必须升版本号
- 旧版本弃用前提前公告，给消费方足够迁移时间

---

## Program.cs 标准结构

```csharp
var builder = WebApplication.CreateBuilder(args);

// ── 阶段一：服务注册（顺序不影响功能）──────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddApiVersioning();

// 注册各层依赖
builder.Services.AddDbContext<AppDbContext>(...);
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// 注册认证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(...);
builder.Services.AddAuthorization();

var app = builder.Build();

// ── 阶段二：中间件管道（顺序严格，不可调换）────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();   // 必须在 UseAuthorization 之前
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
```

---

## 数据管理策略

| 场景 | 策略 |
|---|---|
| 跨服务数据共享 | API 调用或事件驱动（严禁跨库 JOIN） |
| 分布式事务 | Saga 模式（Choreography 或 Orchestration） |
| 读写分离 | CQRS（Command Query Responsibility Segregation） |
| 数据聚合 | API Gateway 层聚合，或专用聚合服务 |
| 最终一致性 | 通过消息队列异步同步，接受短暂不一致 |
| 数据重复 | 允许适度冗余，避免强依赖跨服务查询 |

---

## 安全规范

```csharp
// JWT 验证（每个服务自行验证 Token）
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration["Auth:Authority"];
        options.Audience  = configuration["Auth:Audience"];
    });
```

- **认证**：OAuth2，`Users.Microservice` 负责发放 JWT Token
- **授权**：各服务自行验证 JWT，细粒度权限用 Policy
- **传输加密**：所有服务间通信启用 TLS/SSL
- **敏感数据**：静态加密，严禁明文存储密码、密钥、连接字符串
- **API Gateway**：统一对外暴露，内部服务端口不直接对外开放

---

## CI/CD 规范（计划）

每个微服务拥有独立的流水线，互不阻塞：

```
Push to main
  └─▶ 单元测试
  └─▶ 集成测试
  └─▶ Build Docker Image
  └─▶ Push to Container Registry
  └─▶ Deploy（独立，不影响其他服务）
```

工具：GitHub Actions / Azure DevOps

---

## 测试规范

| 测试类型 | 目标 | 工具 |
|---|---|---|
| 单元测试 | 单个 Service / Repository 方法 | xUnit + Moq |
| 集成测试 | Controller → Service → DB 完整链路 | WebApplicationFactory |
| 契约测试 | 验证服务 API 符合约定契约 | Pact |
| 端到端测试 | 跨服务完整业务流程 | 手动 / Playwright |

---

## 微服务 vs 单体架构（学习对比）

| 维度 | 单体架构 | 微服务架构 |
|---|---|---|
| 部署 | 整体重部署 | 各服务独立部署 |
| 扩容 | 整体扩容，资源浪费 | 按需扩容单个服务 |
| 技术栈 | 统一技术栈 | 各服务可独立选型 |
| 故障影响 | 一处故障影响全局 | 故障隔离，影响范围小 |
| 开发协作 | 代码冲突频繁 | 团队独立并行开发 |
| 代码复杂度 | 单一大型代码库 | 各服务小而专注 |
| 运维复杂度 | 相对简单 | 需要容器化、服务发现、分布式追踪 |
| 数据管理 | 共享数据库，简单 | 各自数据库，一致性需额外处理 |
| 测试 | 相对简单 | 集成测试和契约测试复杂度高 |

---

## 当前开发进度

### 基础结构搭建
- [ ] Products.Microservice — Clean Architecture 四层项目创建
- [ ] Users.Microservice — Clean Architecture 四层项目创建
- [ ] Orders.Microservice — Clean Architecture 四层项目创建
- [ ] Payments.Microservice — Clean Architecture 四层项目创建

### 基础功能实现
- [ ] 各服务 Entity + Repository 定义（Domain 层）
- [ ] 各服务 DTO + Service 实现（Core 层）
- [ ] 各服务 EF Core DbContext + Migration（Infrastructure 层）
- [ ] 各服务 CRUD Controller + Swagger（UI 层）
- [ ] JWT 认证接入

### 进阶功能
- [ ] Polly 弹性策略（Retry + Circuit Breaker + Timeout）
- [ ] Serilog 结构化日志 + CorrelationId
- [ ] OpenTelemetry 分布式追踪
- [ ] API 版本控制（Asp.Versioning）
- [ ] RabbitMQ 异步通信
- [ ] API Gateway（Ocelot / YARP）

### 部署
- [ ] Docker Compose 本地编排
- [ ] Kubernetes 部署配置（K8s Manifests）

---

## 常见陷阱与注意事项

1. **不要过早拆分** — 先把 `Products.Microservice` 做好，验证架构后再复制到其他服务
2. **数据库绝对不能共享** — 这是微服务的底线，共享数据库等于披着微服务皮的单体
3. **服务间同步调用链不能太长** — A→B→C→D 的同步链会叠加延迟，超过 3 跳考虑改异步
4. **CorrelationId 从第一天就要加** — 否则出了问题无法追踪跨服务的完整请求链路
5. **Polly 从第一天就要加** — 一个无超时的外部调用会把所有调用线程全部堵死
6. **不要在 Controller 里写业务逻辑** — Controller 只负责接收请求、调用 Service、返回结果
7. **appsettings.json 不存密钥** — 连接字符串、JWT Secret 使用环境变量或 Azure Key Vault

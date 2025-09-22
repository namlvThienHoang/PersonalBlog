# Hướng dẫn Setup và Chạy Personal Blog API

## 🚀 Các bước thiết lập

### 1. Tạo Project và Cài đặt Packages

```bash
# Tạo solution và project
dotnet new webapi -n PersonalBlog.API
cd PersonalBlog.API

# Cài đặt các packages cần thiết
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

### 2. Cấu hình Connection String

Trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonalBlogDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

**Các tùy chọn Connection String:**

- **LocalDB**: `Server=(localdb)\\mssqllocaldb;Database=PersonalBlogDb;Trusted_Connection=true;TrustServerCertificate=true`
- **SQL Server Express**: `Server=.\\SQLEXPRESS;Database=PersonalBlogDb;Trusted_Connection=true;TrustServerCertificate=true`
- **SQL Server với Authentication**: `Server=localhost;Database=PersonalBlogDb;User Id=sa;Password=YourPassword123;TrustServerCertificate=true`

### 3. Tạo và Chạy Migrations

```bash
# Tạo migration đầu tiên
dotnet ef migrations add InitialCreate

# Cập nhật database
dotnet ef database update

# Nếu muốn xóa database và tạo lại
dotnet ef database drop
dotnet ef database update
```

### 4. Chạy Application

```bash
# Development mode
dotnet run

# Với hot reload
dotnet watch run
```

API sẽ chạy tại:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001` (root path)

## 📋 API Endpoints Documentation

### BlogPosts Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/blogposts` | Get all blog posts (với filtering, search, pagination) |
| GET | `/api/blogposts/{id}` | Get blog post by ID |
| GET | `/api/blogposts/url-handle/{urlHandle}` | Get blog post by URL handle |
| POST | `/api/blogposts` | Create new blog post |
| PUT | `/api/blogposts/{id}` | Update blog post |
| DELETE | `/api/blogposts/{id}` | Delete blog post |

**Query Parameters cho GET /api/blogposts:**
- `searchTerm`: Tìm kiếm trong Title, Content, ShortDescription
- `categoryId`: Lọc theo Category
- `tagIds`: Lọc theo Tags (có thể multiple: `tagIds=id1&tagIds=id2`)
- `isVisible`: Lọc bài viết visible/hidden
- `pageNumber`: Số trang (default: 1)
- `pageSize`: Số items per page (default: 10, max: 100)
- `sortBy`: Sắp xếp theo (`PublishedDate`, `Title`)
- `isDescending`: Sắp xếp giảm dần (default: true)

**Ví dụ:**
```
GET /api/blogposts?searchTerm=dotnet&pageNumber=1&pageSize=5&sortBy=PublishedDate&isDescending=true
```

### Categories Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/categories` | Get all categories |
| GET | `/api/categories/{id}` | Get category by ID |
| POST | `/api/categories` | Create new category |
| PUT | `/api/categories/{id}` | Update category |
| DELETE | `/api/categories/{id}` | Delete category |

### Tags Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tags` | Get all tags |
| GET | `/api/tags/{id}` | Get tag by ID |

## 🔧 Cấu hình CORS cho Frontend

API đã được cấu hình CORS để hỗ trợ Next.js app:

```csharp
// Development: cho phép localhost:3000
// Production: cấu hình domain cụ thể
```

Để thay đổi CORS settings, edit trong `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://yourdomain.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

## 📊 Sample Data

Database sẽ tự động seed với data mẫu:

**Categories:**
- Technology
- Programming  
- Web Development

**Tags:**
- C#
- .NET
- Entity Framework
- API
- React

## 🧪 Testing API với Sample Requests

### 1. Tạo Blog Post mới

```json
POST /api/blogposts
Content-Type: application/json

{
  "title": "Getting Started with .NET 8",
  "shortDescription": "Learn the basics of .NET 8 development",
  "content": "This is a comprehensive guide to .NET 8...",
  "featuredImageUrl": "https://example.com/image.jpg",
  "urlHandle": "getting-started-dotnet-8",
  "author": "John Doe",
  "publishedDate": "2024-01-15T10:00:00Z",
  "isVisible": true,
  "categoryId": "f1234567-8901-2345-6789-012345678902",
  "tagIds": [
    "t1234567-8901-2345-6789-012345678901",
    "t1234567-8901-2345-6789-012345678902"
  ]
}
```

### 2. Search Blog Posts

```
GET /api/blogposts?searchTerm=dotnet&categoryId=f1234567-8901-2345-6789-012345678902&pageNumber=1&pageSize=5
```

### 3. Tạo Category mới

```json
POST /api/categories
Content-Type: application/json

{
  "name": "Mobile Development",
  "urlHandle": "mobile-development"
}
```

## ⚠️ Troubleshooting

### Lỗi Database Connection

1. **Kiểm tra SQL Server đã chạy chưa**
2. **Với LocalDB**: Đảm bảo đã cài SQL Server LocalDB
3. **Kiểm tra Connection String** đúng format
4. **Firewall**: Đảm bảo port SQL Server không bị block

### Lỗi Migration

```bash
# Xóa migration
dotnet ef migrations remove

# Xóa database
dotnet ef database drop

# Tạo lại từ đầu
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Lỗi CORS

Nếu frontend không gọi được API:
1. Kiểm tra URL frontend trong CORS policy
2. Đảm bảo `UseCors()` được gọi trước `UseAuthorization()`
3. Kiểm tra request headers từ frontend

## 📈 Performance Tips

1. **Database Indexing**: Các index quan trọng đã được tự động tạo
2. **Pagination**: Luôn sử dụng pagination cho list APIs
3. **Caching**: Có thể thêm Redis cache sau này
4. **Async/Await**: Tất cả database calls đã sử dụng async

## 🔒 Security Considerations

1. **Input Validation**: Sử dụng Data Annotations và ModelState
2. **SQL Injection**: Entity Framework tự động prevent
3. **XSS Protection**: Frontend cần sanitize HTML content
4. **Authentication**: Có thể thêm JWT auth sau này

## 🚀 Next Steps

1. **Authentication & Authorization**: Thêm JWT authentication
2. **File Upload**: API để upload images
3. **Caching**: Implement Redis caching
4. **Logging**: Thêm structured logging với Serilog
5. **Unit Tests**: Viết unit tests cho repositories và controllers

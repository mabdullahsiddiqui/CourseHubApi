# CourseHub API

A .NET 8 Web API for course management with authentication, enrollment, and payment processing.

## Features

- 🔐 JWT Authentication
- 📚 Course Management
- 👥 User Enrollment
- 💳 Stripe Payment Integration
- 📊 SQLite Database
- 📖 Swagger API Documentation

## Local Development

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or VS Code

### Setup

1. Clone the repository
```bash
git clone <your-repo-url>
cd CourseHubApi
```

2. Restore dependencies
```bash
dotnet restore
```

3. Run database migrations
```bash
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

5. Access the API at `https://localhost:7000` or `http://localhost:5000`
6. Access Swagger UI at `https://localhost:7000/swagger`

## Azure Deployment

This project is configured for automatic deployment to Azure App Service via GitHub Actions.

### Prerequisites

1. Azure subscription
2. GitHub repository
3. Azure App Service created

### Setup Steps

1. **Create Azure App Service**
   - Go to Azure Portal
   - Create a new App Service
   - Choose .NET 8 runtime
   - Note the app name (e.g., `coursehub-api`)

2. **Get Publish Profile**
   - In Azure Portal, go to your App Service
   - Navigate to "Deployment Center"
   - Download the publish profile

3. **Configure GitHub Secrets**
   - Go to your GitHub repository
   - Navigate to Settings > Secrets and variables > Actions
   - Add a new secret named `AZURE_WEBAPP_PUBLISH_PROFILE`
   - Paste the content of the downloaded publish profile

4. **Update App Name**
   - Edit `.github/workflows/azure-deploy.yml`
   - Change `AZURE_WEBAPP_NAME` to match your Azure App Service name

5. **Push to GitHub**
   ```bash
   git add .
   git commit -m "Add Azure deployment workflow"
   git push origin main
   ```

### Environment Variables

Set these in Azure App Service Configuration:

- `ConnectionStrings:DefaultConnection` - Your database connection string
- `JwtSettings:Key` - JWT signing key
- `JwtSettings:Issuer` - JWT issuer
- `JwtSettings:Audience` - JWT audience
- `Stripe:SecretKey` - Stripe secret key

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user

### Courses
- `GET /api/courses` - Get all courses
- `POST /api/courses` - Create new course
- `GET /api/courses/{id}` - Get course by ID
- `PUT /api/courses/{id}` - Update course
- `DELETE /api/courses/{id}` - Delete course

### Enrollments
- `GET /api/enrollments` - Get user enrollments
- `POST /api/enrollments` - Enroll in course

### Payments
- `POST /api/payments/create-payment-intent` - Create payment intent

## Testing

Run the tests:
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License

This project is licensed under the MIT License. 
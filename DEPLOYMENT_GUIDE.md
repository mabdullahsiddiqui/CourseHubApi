# Azure App Service Deployment Guide

This guide will walk you through deploying your CourseHub API to Azure App Service using GitHub Actions.

## Prerequisites

1. **Azure Subscription**: You need an active Azure subscription
2. **GitHub Account**: Your code should be in a GitHub repository
3. **Azure CLI** (optional): For easier Azure management

## Step 1: Create Azure App Service

### Option A: Using Azure Portal

1. Go to [Azure Portal](https://portal.azure.com)
2. Click "Create a resource"
3. Search for "App Service" and select it
4. Click "Create"
5. Fill in the details:
   - **Resource Group**: Create new or use existing
   - **Name**: `coursehub-api` (or your preferred name)
   - **Publish**: Code
   - **Runtime stack**: .NET 8 (LTS)
   - **Operating System**: Windows
   - **Region**: Choose your preferred region
   - **App Service Plan**: Create new (Basic B1 or higher recommended)
6. Click "Review + create" then "Create"

### Option B: Using Azure CLI

```bash
# Login to Azure
az login

# Create resource group
az group create --name CourseHubRG --location eastus

# Create App Service Plan
az appservice plan create --name CourseHubPlan --resource-group CourseHubRG --sku B1

# Create App Service
az webapp create --name coursehub-api --resource-group CourseHubRG --plan CourseHubPlan --runtime "DOTNETCORE:8.0"
```

## Step 2: Get Publish Profile

1. In Azure Portal, go to your App Service
2. Navigate to "Deployment Center"
3. Click "Download publish profile"
4. Save the file (it's an XML file)

## Step 3: Configure GitHub Repository

### 3.1 Push Your Code to GitHub

If you haven't already, push your code to GitHub:

```bash
# Add your GitHub repository as remote
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git

# Push to GitHub
git push -u origin master
```

### 3.2 Add GitHub Secret

1. Go to your GitHub repository
2. Navigate to Settings > Secrets and variables > Actions
3. Click "New repository secret"
4. Name: `AZURE_WEBAPP_PUBLISH_PROFILE`
5. Value: Copy the entire content of the downloaded publish profile XML file
6. Click "Add secret"

### 3.3 Update App Name

Edit `.github/workflows/azure-deploy.yml` and change the app name:

```yaml
env:
  AZURE_WEBAPP_NAME: coursehub-api  # Change this to your app name
```

## Step 4: Configure Environment Variables

In Azure Portal, go to your App Service:

1. Navigate to "Configuration" > "Application settings"
2. Add these environment variables:

```
ConnectionStrings__DefaultConnection = Data Source=/home/site/wwwroot/coursehub.db
JwtSettings__Key = YourSuperSecretKeyHere1234567890!!##
JwtSettings__Issuer = CourseHubApi
JwtSettings__Audience = CourseHubUsers
JwtSettings__DurationInMinutes = 60
Stripe__SecretKey = sk_test_your_stripe_secret_key
```

3. Click "Save"

## Step 5: Deploy

1. Commit and push your changes:
```bash
git add .
git commit -m "Configure Azure deployment"
git push
```

2. Go to your GitHub repository
3. Navigate to "Actions" tab
4. You should see the deployment workflow running
5. Wait for it to complete

## Step 6: Verify Deployment

1. Go to your Azure App Service URL: `https://your-app-name.azurewebsites.net`
2. Test the Swagger UI: `https://your-app-name.azurewebsites.net/swagger`
3. Test the API endpoints

## Troubleshooting

### Common Issues

1. **Build Fails**
   - Check the GitHub Actions logs
   - Ensure all dependencies are properly referenced

2. **App Won't Start**
   - Check the Azure App Service logs
   - Verify environment variables are set correctly

3. **Database Issues**
   - SQLite database will be created automatically on first run
   - Ensure the app has write permissions to `/home/site/wwwroot/`

4. **Authentication Issues**
   - Verify JWT settings are configured correctly
   - Check that the secret key is properly set

### Useful Commands

```bash
# View Azure App Service logs
az webapp log tail --name coursehub-api --resource-group CourseHubRG

# Restart the app
az webapp restart --name coursehub-api --resource-group CourseHubRG

# View app settings
az webapp config appsettings list --name coursehub-api --resource-group CourseHubRG
```

## Security Considerations

1. **JWT Key**: Use a strong, unique key in production
2. **Stripe Keys**: Use test keys for development, live keys for production
3. **Database**: Consider using Azure SQL Database for production
4. **HTTPS**: Azure App Service provides HTTPS by default

## Cost Optimization

- Use Basic B1 plan for development
- Consider scaling down during non-business hours
- Monitor usage in Azure Portal

## Next Steps

1. Set up custom domain (optional)
2. Configure SSL certificates
3. Set up monitoring and alerts
4. Configure backup strategies
5. Set up CI/CD for multiple environments (dev, staging, prod) 
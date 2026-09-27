$ErrorActionPreference = "Stop"

Write-Host "Updating Identity database..." -ForegroundColor Cyan
dotnet ef database update `
    --context IdentityDbContext `
    --project src/Modules/Identity/Identity.Api `
    --startup-project src/Api

Write-Host "Updating Cart database..." -ForegroundColor Cyan
dotnet ef database update `
    --context CartDbContext `
    --project src/Modules/Cart/Cart.Infrastructure `
    --startup-project src/Api

Write-Host "Updating Catalog database..." -ForegroundColor Cyan
dotnet ef database update `
    --context CatalogDbContext `
    --project src/Modules/Catalog/Catalog.Infrastructure `
    --startup-project src/Api

Write-Host "Updating Inventory database..." -ForegroundColor Cyan
dotnet ef database update `
    --context InventoryDbContext `
    --project src/Modules/Inventory/Inventory.Infrastructure `
    --startup-project src/Api

Write-Host "Updating Payment database..." -ForegroundColor Cyan
dotnet ef database update `
    --context PaymentDbContext `
    --project src/Modules/Payment/Payment.Infrastructure `
    --startup-project src/Api

Write-Host "Updating Order database..." -ForegroundColor Cyan
dotnet ef database update `
    --context OrderDbContext `
    --project src/Modules/Order/Order.Infrastructure `
    --startup-project src/Api

Write-Host "Updating Notification database..." -ForegroundColor Cyan
dotnet ef database update `
    --context NotificationDbContext `
    --project src/Modules/Notification/Notification.Infrastructure `
    --startup-project src/Api

Write-Host "`nAll databases are up to date!" -ForegroundColor Green
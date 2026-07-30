@echo off
dotnet publish /p:EnvironmentName=Staging E:\_Project\CSharp\WEB\Btech\_repo\Inventory\Inventory_Api\IQT_FSD_2026.WebAPI\IQT_FSD_2026.WebAPI.csproj --configuration Release --output E:\_Project\CSharp\WEB\Btech\_repo\publish\Inventory\  --self-contained false
pause
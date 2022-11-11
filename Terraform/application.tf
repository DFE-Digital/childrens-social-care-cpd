resource "azurerm_service_plan" "service-plan" {
  name                = "s185d01-csc-cpd-app-service-plan"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "B1"
  tags                = azurerm_resource_group.rg.tags
}

resource "azurerm_linux_web_app" "linux-web-app" {
  name                = "s185d01-chidrens-social-care-cpd-app-service"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  service_plan_id     = azurerm_service_plan.service-plan.id

  app_settings = {
    CPD_KEYVAULTENDPOINT       = var.cpd_keyvaultendpoint
    CPD_CLIENTID               = var.cpd_client_id
    CPD_CLIENTSECRET           = var.cpd_client_secret
    CPD_TENANTID               = var.tenant_id
    CPD_CONTENTFUL_ENVIRONMENT = "dev"
    DOCKER_ENABLE_CI           = "true"
  }

  site_config {
    application_stack {
      docker_image     = "ghcr.io/dfe-digital/childrens-social-care-cpd"
      docker_image_tag = "master"
    }
  }

  tags = azurerm_resource_group.rg.tags
}

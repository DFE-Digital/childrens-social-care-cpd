resource "azurerm_service_plan" "service-plan" {
  name                = var.service_plan_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = var.service_plan_sku[terraform.workspace]
  tags                = data.azurerm_resource_group.rg.tags
}

resource "azurerm_linux_web_app" "linux-web-app" {
  name                = var.web_app_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  service_plan_id     = azurerm_service_plan.service-plan.id

  app_settings = {
    CPD_KEYVAULTENDPOINT       = var.cpd_keyvaultendpoint
    CPD_CLIENTID               = var.cpd_client_id
    CPD_CLIENTSECRET           = var.cpd_client_secret
    CPD_TENANTID               = var.tenant_id
    CPD_CONTENTFUL_ENVIRONMENT = lower(terraform.workspace)
    DOCKER_ENABLE_CI           = "true"
  }

  site_config {
    application_stack {
      docker_image     = "ghcr.io/dfe-digital/childrens-social-care-cpd"
      docker_image_tag = "master"
      # docker_image_tag = lower(terraform.workspace)
    }
  }

  tags = data.azurerm_resource_group.rg.tags
}

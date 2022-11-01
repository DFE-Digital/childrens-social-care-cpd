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
    CPD_KEYVAULTENDPOINT = var.cpd_keyvaultendpoint
    CPD_CLIENTID         = var.cpd_client_id
    CPD_CLIENTSECRET     = var.cpd_client_secret
    CPD_TENANTID         = var.tenant_id
  }

  identity {
    type = "SystemAssigned"
  }

  site_config {
    container_registry_use_managed_identity = true

    ip_restriction {
      name       = "AGW-PIP"
      ip_address = "${azurerm_public_ip.pip1.ip_address}/32"
      priority   = 1000
      action     = "Allow"
    }

    ip_restriction {
      name                      = "AGW-Subnet"
      virtual_network_subnet_id = azurerm_subnet.frontend.id
      priority                  = 2000
      action                    = "Allow"
    }

    ip_restriction {
      name       = "s185d01-c-s-c-cpd-02-nsg"
      ip_address = "0.0.0.0/0"
      priority   = 3000
      action     = "Deny"
    }

    application_stack {
      docker_image     = "s185d01coreacr.azurecr.io/dfe-digital/childrens-social-care-cpd"
      docker_image_tag = "master"
    }
  }

  tags = azurerm_resource_group.rg.tags
}

data "azurerm_container_registry" "acr" {
  name                = "s185d01coreacr"
  resource_group_name = "s185d01-core"
}

resource "azurerm_role_assignment" "acr_Pull" {
  scope                = data.azurerm_container_registry.acr.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_linux_web_app.linux-web-app.identity[0].principal_id
}

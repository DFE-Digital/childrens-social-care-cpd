resource "azurerm_application_gateway" "appgw" {
  name                = var.appgw_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location

  sku {
    name     = var.appgw_tier[terraform.workspace]
    tier     = var.appgw_tier[terraform.workspace]
    capacity = 2
  }

  gateway_ip_configuration {
    name      = var.gateway_ip_configuration[terraform.workspace]
    subnet_id = azurerm_subnet.frontend.id
  }

  frontend_port {
    name = var.frontend_port_name[terraform.workspace]
    port = 80
  }

  frontend_port {
    name = var.frontend_ssl_port_name[terraform.workspace]
    port = 443
  }

  frontend_ip_configuration {
    name                 = var.frontend_ip_configuration_name[terraform.workspace]
    public_ip_address_id = data.azurerm_public_ip.pip1.id
  }

  backend_address_pool {
    name  = var.backend_address_pool_name[terraform.workspace]
    fqdns = [azurerm_linux_web_app.linux-web-app.default_hostname]
  }

  # backend_http_settings {
  #   name                                = var.http_setting_name[terraform.workspace]
  #   pick_host_name_from_backend_address = true
  #   cookie_based_affinity               = "Disabled"
  #   path                                = "/"
  #   port                                = 80
  #   protocol                            = "Http"
  #   request_timeout                     = 60
  # }

  backend_http_settings {
    name                                = var.https_setting_name[terraform.workspace]
    pick_host_name_from_backend_address = true
    cookie_based_affinity               = "Disabled"
    path                                = "/"
    port                                = 443
    protocol                            = "Https"
    request_timeout                     = 60
  }

  http_listener {
    name                           = var.listener_name[terraform.workspace]
    frontend_ip_configuration_name = var.frontend_ip_configuration_name[terraform.workspace]
    frontend_port_name             = var.frontend_port_name[terraform.workspace]
    protocol                       = "Http"
  }

  http_listener {
    name                           = var.ssl_listener_name[terraform.workspace]
    frontend_ip_configuration_name = var.frontend_ip_configuration_name[terraform.workspace]
    frontend_port_name             = var.frontend_ssl_port_name[terraform.workspace]
    protocol                       = "Https"
    ssl_certificate_name           = "develop-child-family-social-work-career"
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [data.azurerm_user_assigned_identity.uai.id]
  }

  ssl_certificate {
    name                = "develop-child-family-social-work-career"
    key_vault_secret_id = "https://cpd-key-vault.vault.azure.net/secrets/develop-child-family-social-work-career"
  }

  ssl_policy {
    disabled_protocols = ["TLSv1_0", "TLSv1_1"]
  }

  redirect_configuration {
    name                 = var.redirect_config_name[terraform.workspace]
    redirect_type        = "Permanent"
    include_path         = true
    include_query_string = true
    target_listener_name = var.ssl_listener_name[terraform.workspace]
  }

  request_routing_rule {
    name                        = var.request_routing_rule_name[terraform.workspace]
    rule_type                   = "Basic"
    redirect_configuration_name = var.redirect_config_name[terraform.workspace]
    priority                    = 2000
    http_listener_name          = var.listener_name[terraform.workspace]
    # backend_address_pool_name  = var.backend_address_pool_name[terraform.workspace]
    # backend_http_settings_name = var.http_setting_name[terraform.workspace]
  }

  request_routing_rule {
    name                       = var.request_ssl_routing_rule_name[terraform.workspace]
    rule_type                  = "Basic"
    priority                   = 2001
    http_listener_name         = var.ssl_listener_name[terraform.workspace]
    backend_address_pool_name  = var.backend_address_pool_name[terraform.workspace]
    backend_http_settings_name = var.https_setting_name[terraform.workspace]
  }

  probe {
    name                                      = var.appgw_probe[terraform.workspace]
    pick_host_name_from_backend_http_settings = true
    path                                      = "/"
    interval                                  = 30
    timeout                                   = 30
    unhealthy_threshold                       = 3
    protocol                                  = "Http"
  }

  probe {
    name                                      = var.appgw_ssl_probe[terraform.workspace]
    pick_host_name_from_backend_http_settings = true
    path                                      = "/"
    interval                                  = 30
    timeout                                   = 30
    unhealthy_threshold                       = 3
    protocol                                  = "Https"
  }

  private_link_configuration {
    name = var.private_link_name[terraform.workspace]
    ip_configuration {
      name                          = var.private_link_ip_conf_name[terraform.workspace]
      subnet_id                     = azurerm_subnet.backend.id
      private_ip_address_allocation = "Dynamic"
      primary                       = true
    }
  }

  tags = data.azurerm_resource_group.rg.tags
}

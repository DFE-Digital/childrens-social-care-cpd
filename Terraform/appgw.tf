resource "azurerm_application_gateway" "appgw" {
  name                = "s185d01-csc-cpd-app-gateway"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location

  #  firewall_policy_id = azurerm_web_application_firewall_policy.appfirewall.id

  sku {
    name     = "Standard_v2"
    tier     = "Standard_v2"
    capacity = 2
  }

  gateway_ip_configuration {
    name      = "s185d01-gateway-ip-configuration"
    subnet_id = azurerm_subnet.frontend.id
  }

  frontend_port {
    name = var.frontend_port_name
    port = 80
  }

  frontend_ip_configuration {
    name                 = var.frontend_ip_configuration_name
    public_ip_address_id = azurerm_public_ip.pip1.id
  }

  backend_address_pool {
    name  = var.backend_address_pool_name
    fqdns = [azurerm_linux_web_app.linux-web-app.default_hostname]
  }

  backend_http_settings {
    name                                = var.http_setting_name
    pick_host_name_from_backend_address = true
    cookie_based_affinity               = "Disabled"
    path                                = "/"
    port                                = 80
    protocol                            = "Http"
    request_timeout                     = 60
  }

  http_listener {
    name                           = var.listener_name
    frontend_ip_configuration_name = var.frontend_ip_configuration_name
    frontend_port_name             = var.frontend_port_name
    protocol                       = "Http"
  }

  request_routing_rule {
    name                       = var.request_routing_rule_name
    rule_type                  = "Basic"
    priority                   = 2000
    http_listener_name         = var.listener_name
    backend_address_pool_name  = var.backend_address_pool_name
    backend_http_settings_name = var.http_setting_name
  }

  probe {
    name                                      = "s185d01AGProbe"
    pick_host_name_from_backend_http_settings = true
    path                                      = "/"
    interval                                  = 30
    timeout                                   = 30
    unhealthy_threshold                       = 3
    protocol                                  = "Http"
  }

  tags = azurerm_resource_group.rg.tags

  # depends_on = [
  #   azurerm_network_security_group.nsg,
  #   azurerm_subnet_network_security_group_association.blockall
  # ]
}

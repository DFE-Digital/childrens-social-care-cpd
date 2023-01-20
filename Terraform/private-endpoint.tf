resource "azurerm_private_dns_zone" "dnsprivatezone" {
  name                = "privatelink.azurewebsites.net"
  resource_group_name = data.azurerm_resource_group.rg.name
  tags                = data.azurerm_resource_group.rg.tags
}

resource "azurerm_private_dns_zone_virtual_network_link" "dnszonelink" {
  name                  = var.private_dns_zone_vn_link_name[terraform.workspace]
  resource_group_name   = data.azurerm_resource_group.rg.name
  private_dns_zone_name = azurerm_private_dns_zone.dnsprivatezone.name
  virtual_network_id    = data.azurerm_virtual_network.vnet1.id
  tags                  = data.azurerm_resource_group.rg.tags
}

resource "azurerm_private_endpoint" "privateendpoint" {
  name                = var.private_endpoint_name[terraform.workspace]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  subnet_id           = azurerm_subnet.backend.id

  private_dns_zone_group {
    name                 = var.private_dns_zone_group_name[terraform.workspace]
    private_dns_zone_ids = [azurerm_private_dns_zone.dnsprivatezone.id]
  }

  private_service_connection {
    name                           = var.private_endpoint_conn_name[terraform.workspace]
    private_connection_resource_id = azurerm_linux_web_app.linux-web-app.id
    subresource_names              = ["sites"]
    is_manual_connection           = false
  }

  tags = data.azurerm_resource_group.rg.tags
}
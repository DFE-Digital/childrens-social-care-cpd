import { Given, Then, When } from "@badeball/cypress-cucumber-preprocessor";

Given("a user has clicked on {string}", (id) => {
    cy.visitcpdhomepage();
    cy.get('[datatestid="' + id + '"]').click();
});

When("user clicks on {string}", (id) => {
    cy.get('[datatestid="' + id + '"]').click();
});

Then("the page has a external link to {string}", (link) => {
    cy.get("a[href]")                           
        .each($el => {
            cy.wrap($el.attr('href'), { log: false }, link)  
            
        })
});
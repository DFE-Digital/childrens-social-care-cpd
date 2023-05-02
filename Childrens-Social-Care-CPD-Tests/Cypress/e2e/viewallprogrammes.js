import { Given, Then, When } from "@badeball/cypress-cucumber-preprocessor";

Given("a user has clicked on {string}", (id) => {
    cy.visitcpdhomepage();
    cy.get('[datatestid="Practitioners"]').click();
    cy.get('[datatestid="' + id + '"]').click();
});


Then("the heading should say {string}", (heading) => {
    cy.get("h1").should("contain.text", heading);
});

When("user clicks on {string}", (id) => {
    cy.get('[datatestid="' + id + '"]').click();
});

Then("the sub heading should say {string}", (heading) => {
    cy.get("h2").should("contain.text", heading);
});
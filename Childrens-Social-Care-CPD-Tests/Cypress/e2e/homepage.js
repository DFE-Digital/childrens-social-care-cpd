import { Given, Then, When } from "@badeball/cypress-cucumber-preprocessor";

Given("a user has arrived on the home page", () => {
    cy.visitcpdhomepage();
});


Then("the heading should say {string}", (heading) => {
    cy.get("h1").should("contain.text", heading);
});

Then("the sub heading should say {string}", (heading) => {
    cy.get("h2").should("contain.text", heading);
});

Then("the link should exists to {string}", (id) => {
    cy.get('[datatestid="' + id + '"]').should("exist");
});
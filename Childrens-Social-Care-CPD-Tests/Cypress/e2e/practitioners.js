import { Given, Then, When } from "@badeball/cypress-cucumber-preprocessor";

Given("a user has arrived on the home page and click Practitioners link", () => {
    cy.visitcpdhomepage();
    cy.get('[datatestid="Practitioners"]').click();
});

Then("the heading should say {string}", (heading) => {
    cy.get("h1").should("contain.text", heading);
});

Then("the link should exists to {string}", (id) => {
    cy.get('[datatestid="' + id + '"]').should("exist");
});

Then("the sub heading should say {string}", (heading) => {
    cy.get("h2").should("contain.text", heading);
});
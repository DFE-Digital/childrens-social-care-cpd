Feature: Leaders Page Tests
 Scenario: Leaders heading is 'Programmes for leaders'
   Given a user has clicked on 'Leaders'
    Then the heading should say 'Programmes for leaders'

 Scenario: Pathway 4: practice leaders from leaders link opens Successfully
    Given a user has clicked on 'Leaders'
    When user clicks on ' Pathway 4: practice leaders'
    Then the heading should say 'Pathway 4: practice leaders'

 Scenario: Upon: aspirant directors programme from leaders link opens Successfully
    Given a user has clicked on 'Leaders'
    When user clicks on 'Upon: aspirant directors programme'
    Then the heading should say 'upon: aspirant directors programme'

 Scenario: Upon: new directors programme from leaders link opens Successfully
    Given a user has clicked on 'Leaders'
    When user clicks on 'Upon: new directors programme'
    Then the heading should say 'upon: new directors programme'
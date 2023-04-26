Feature:  Experienced Pracitioners Page Tests
 Scenario: experienced practitioners heading is 'Programmes for experienced practitioners'
   Given a user has clicked on 'Experienced practitioners'
    Then the heading should say 'Programmes for experienced practitioners'

 Scenario: Pathway 1: practice supervisors from experienced practioners link opens Successfully
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 1: practice supervisors'
    Then the heading should say 'Pathway 1: practice supervisors'

 Scenario: Pathway 2: middle managers from experienced practioners link opens Successfully
    Given a user has clicked on 'Experienced practitioners'
    When user clicks on 'Pathway 2: middle managers'
    Then the heading should say 'Pathway 2: middle managers'
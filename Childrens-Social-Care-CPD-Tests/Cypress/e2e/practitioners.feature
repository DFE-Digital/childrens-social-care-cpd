Feature:  Practitioners Link Tests
 Scenario: practitoners page heading is 'Programmes for practitioners'
    Given a user has clicked on 'Practitioners'
    Then the heading should say 'Programmes for practitioners'
    
 Scenario: practitoners page has link to ' Assessed and supported year in employment'
    Given a user has clicked on 'Practitioners'
    When user clicks on ' Assessed and supported year in employment'
    Then the heading should say 'Assessed and supported year in employment (ASYE)'

 Scenario: practitoners page sub heading is 'Professional development for practitionerse'
    Given a user has clicked on 'Practitioners'
     Then the sub heading should say 'Professional development for practitioners'

 Scenario: practitoners page  link open page with heading 'Develop your social work practice'
    Given a user has clicked on 'Practitioners'
      When user clicks on sub heading
     Then page opens with a heading  says 'Develop your social work practice'
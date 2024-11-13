Feature: Calculator
Simple calculator for adding **two** numbers

@Calculator
Scenario: Add two numbers
    Given the Calculator application is open
    When I enter "One"
    And I press the "Plus" button
    And I enter "Two"
    And I press the "Equals" button
    Then the result should be "3"
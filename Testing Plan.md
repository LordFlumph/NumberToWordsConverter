## Overview
The goal of this project was to convert numerical input into its equivalent English words. Given the complexity of number formats, large value ranges, and the wide range of potential input, thorough testing was essential to ensure the system’s output is correct regardless of the number given. Testing was focused on verifying that the system produced accurate output across a wide variety of input, and as such, tests were conducted using input that would be normal as well as input that would only be provided by someone intentionally trying to break the system.

These tests included simple numbers, large numbers, small numbers, negative numbers, decimals, commas, as well as various combinations of these. Any issues that were identified throughout testing were immediately addressed and tests were then repeated to ensure that the system could handle any input.

## Automatic Tests
A simple unit test was setup to aid in finding cases where the system fails to correctly interpret an input. A total of 40 inputs were inputted for the test with their intention being to test either the general functionality or edge cases. Thanks to these tests, numerous cases were identified where the system fails to produce the intended output. Most of these failures were simple cases of formatting (e.g. an extra space) which were quickly resolved. The remaining failures were due to not considering certain edge cases, which the tests identified, allowing me to adjust the system accordingly.

Automated testing is limited to the backend only, as thorough testing of the frontend was deemed unnecessary due to its simplicity.

## Manual Tests
In addition to the unit tests, numerous manual tests were conducted in order to test both the system and the frontend. These consisted of providing various input to the frontend and then confirming the output was correct. Apart from during the initial creation of the system, before the unit tests were implemented, these manual tests did not identify any system related issues.

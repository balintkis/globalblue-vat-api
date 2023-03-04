Global Blue VAT API excercise<a name="TOP"></a>
===================

    by Balint Kis v. 0.1, 05.03.2023.
       

## Additional things to consider ##

* The API currently does not perform rounding - this would be something probably implemented on the backend based on specifications (to two decimal points?)
* Not sure if negative prices or VAT rates are things that exists, but for simplicity's sake we just map to absolute values in this case
* It quite likely that the logic for calculating price details (NET/Gross/Vat amount) would be the same for several countries, therefore that section could be refactored out to a more generic helper
* Keeping track of valid VAT rates would probably be persisted to SQL with dedicated endpoints to update per country, with maybe an in-memory cache kept up-to-date via something like SqlTableDependency so that the DB does not need to be touched for each and every request

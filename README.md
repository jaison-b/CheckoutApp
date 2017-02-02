# CheckoutApp
Grocery Co Checkout App

## Usage
```bash
> CheckoutApp --help
CheckoutApp
Copyright (C) 2012 Jaison.B

  -i, --input-file       Required. Order input file

  -p, --product-file     Required. Product catalog file

  -d, --discount-file    Required. Promotion input file

  --help                 Display this help screen.
```

### Testing Executable
Relative to where the executable is built there is a SampleData folder that can used for testing:
```bash
CheckoutApp -i SampleData\orders.txt -p SampleData\products.txt -d SampleData\promotions.txt
```
#### Result:
![alt text](CheckoutAppResult.png "Checkout App result")

## Input file format
__CSV in the only supported format__  
___Note: header row is required for csv files to be processed___
### Products file format [sample]
```
PRODUCT_ID, PRODUCT_NAME, UNIT_PRICE
111, APPLE, 0.75
222, BANANA, 1.00
333, ORANGE, 0.85
444, BOWTIE PASTA, 2.97
555, CHEDDAR CHEESE, 3.99
```
### Promotions file format [sample]
```
PRODUCT_ID, PROMO_TYPE, START_DATE, END_DATE, ELIGIBLE_QUANTITY, PROMO_AMOUNT
111, SalePrice, 2017-01-30, 2017-02-15, 1, 0.50
222, SalePercent, 2017-01-30, 2017-02-15, 1, 10
333, BundleDiscount, 2017-01-30, 2017-02-15, 3, 2.00
444, AddOnPercent, 2017-01-30, 2017-02-15, 1, 50
555, AddOnUnit, 2017-01-30, 2017-02-15, 3, 1
```
List of Promo Types supported currently:  
  1. *SalePrice* - discounted price on sale item for e.g. "Apple sale price ~~$0.75~~ $0.50"
  2. *SalePercent* - discounted percent on sale item for e.g. "10% off on Bananas"
  3. *BundleDiscount* - bundle discounts for e.g. "Buy 3 Oranges for $2.00"
  4. *AddOnPercent* - additional promotion for e.g. "Buy One Get 50% off" promo
  5. *AddOnUnit* - additional promotion for e.g. "Buy One Get One" promo

### Input Orders file format [sample]
```
PRODUCT_ID, UNITS
111, 7
222, 3
111, -2
333, 6
444, 4
555, 4
```
## Design Choices  
  1. Fractional *'UNITS'* is not allowed in the orders input file. It will be rounded up(Math.Ceiling)
  2. Fractional *'ELIGIBLE_QUANTITY'* is not allowed in the promotions input file. It will be rounded down(Math.Floor)
  3. Currently, promotions will be applied/chained in the order specified in promotions file.As a future enhancement we can introduce something like *'PRIORITY'* that will allow us to sort the promotion order.
  4. If a new promotion is being introduced, it needs to be supported by a valid [PromoType](https://github.com/jaison-b/CheckoutApp/blob/master/CheckoutApp/Repository/PromoType.cs) and [Promotion](https://github.com/jaison-b/CheckoutApp/blob/master/CheckoutApp/Models/Promotion.cs) implementation that supports the price calculation.
  5. *'START_DATE'* and *'END_DATE'* is not mandatory and will be defaulted to DateTime.MinValue for START_DATE and DateTime.MaxValue for END_DATE incase not provided in the input file.
  6. All amounts/prices are converted down to cents(lowest unit). This avoids precision issues when handling currency and all calculations are done using the value. Amounts will be formatted back to dollars only on display.
  7. A [decorator](https://en.wikipedia.org/wiki/Decorator_pattern) pattern was used for promotions calculaton. It allows lot of flexibility to add more promotions in the future.
  8. [CartFactory](https://github.com/jaison-b/CheckoutApp/blob/master/CheckoutApp/CartFactory.cs) is reponsible for processing the input orders file and returning promotions wrapped around [IOrderItem](https://github.com/jaison-b/CheckoutApp/blob/master/CheckoutApp/Models/IOrderItem.cs) to calculate pricing.
 
## Dependencies  
_sourced through NuGet_
  1. [CommandLineParser](https://github.com/gsscoder/commandline) 
  2. [CsvHelper](https://joshclose.github.io/CsvHelper)
  3. [Colorful.Console](https://github.com/tomakita/Colorful.Console)
  4. [Moq](https://github.com/Moq/moq4/wiki/Quickstart) - mock library used in tests.

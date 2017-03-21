// Algorithm Separting NAICS Codes into Four General Categories. 

project.category = project.category || {};
project.category.tiers = project.category.tiers || {};

project.category.tiers.categorizeTiers = function(currentCode) {
  var currentTier = findTier(currentCode, 1000000);
  console.log('current tier is', currentTier);

  // Initialize Array to avoid nulls in database
  var tiersArray = [0, 0, 0, 0];

  tiersArray[currentTier - 1] = currentCode;

  console.log(tiersArray);

  for (var i = currentTier - 1; i > 0; i--) {
    var loopDivisor = Math.pow(100, 4 - i);
    // Inserting next tier below (tier 4 to tier 3)
    tiersArray[i - 1] = Math.floor(currentCode / loopDivisor) * loopDivisor; // e.g. 48121316 => 48121300
    console.log(tiersArray);
  }

  return storeTiers(tiersArray);

}

function findTier(currentCode, divisor) {
  if (!divisor) {
    divisor = 1000000;
  } else if (divisor === 1) {
    console.log('Divisor:', divisor);
    return 4; // This line serves as a base case to break recursion.
  }

  var remainder = currentCode % divisor;
  if (remainder === 0) {
    console.log('base case reached with divisor = ', divisor);
    return 5 - ((Math.log10(divisor) + 2) / 2);
  } else {
    return findTier(currentCode, divisor / 100);
  }

}

function storeTiers(tiersArray) {
  var tiersObject = {};

  for (var i = 0; i < tiersArray.length; i++) {
    tiersObject[i + 1] = tiersArray[i].toString();
  }
  return tiersObject;
}



/*
Example:

  Output = 

  {

  1: 48000000 ->  48 x 10^6 [5 - (6 + 2)/2]             
  2: 48120000 ->  48 x 10^4 [5 - (4 + 2)/2]
  3: 48121300 ->  48 x 10^2 [5 - (2 + 2)/2]
  4: 48121316 ->  48 x 10^0 [5 - (0 + 2)/2]
  
  };
  
  
  1 :48000000 ->  48 x 100^3  (4-3) i.e. 3 is the exponent
  2: 48120000 ->  48 x 100^2  (4-2) i.e. 2 is the exponent
  3: 48121300 ->  48 x 100^1  (4-1) i.e. 2 is the exponent
  4: 48121316 ->  48 x 100^0  (4-0) i.e. 2 is the exponent

*/


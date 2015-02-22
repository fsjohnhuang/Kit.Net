(function() {
  var add, sum;

  add = function(a, b) {
    return a + b;
  };

  sum = function(a) {
    return this.s += a;
  };

  test('createCallback', function() {
    equal(lppExt.createCallback(add, 1)(2), 3, '1 + 2 = 3');
    equal(lppExt.createCallback(add, 2)(4), 6, '2 + 4 = 6');
    return equal(lppExt.createCallback(add, 2, 5)(4), 7, '2 + 5 = 7');
  });

  test('createDelegate', function() {
    var ctx1, ctx2;

    ctx1 = {
      s: 1
    };
    ctx2 = {
      s: 2
    };
    equal(lppExt.createDelegate(sum, ctx1)(2), 3, '1 + 2 = 3');
    return equal(lppExt.createDelegate(sum, ctx2)(2), 4, '2 + 2 = 4');
  });

}).call(this);

(function() {
  module('lpp.getConfigVal', {
    setup: function() {
      return this.config = {
        name: 'John',
        age: 21.2,
        sex: true,
        height: 173,
        weight: 150.56,
        hobbies: ['dancing', 'singing'],
        job: 'programmer'
      };
    }
  });

  test('lpp.getConfigVal, propName is string!', function() {
    var age, config, height, hobbies, job, name, sex;

    config = this.config;
    name = lpp.getConfigVal(config, 'name', 'DefaultVal', 'string', function(val) {
      return "hello " + val;
    });
    equal(name, 'hello John', "test property in string type");
    age = lpp.getConfigVal(config, 'age', 'DefaultVal', ['int'], function(val) {
      return "My age is " + val;
    });
    equal(age, 'DefaultVal', "age isn't int");
    sex = lpp.getConfigVal(config, 'sex', 'DefaultVal', 'bool', [true]);
    equal(sex, true, "false stands for men!");
    height = lpp.getConfigVal(config, 'height', 'DefaultVal', ['int', 'string'], function(val) {
      if (lpp.isInt(val)) {
        return "" + val + "cm";
      } else {
        return val;
      }
    });
    equal(height, '173cm', 'testing tall');
    hobbies = lpp.getConfigVal(config, 'hobbies', null, 'Array');
    equal(hobbies, config.hobbies, 'is array');
    job = lpp.getConfigVal(config, 'job', null, 'string', ['programmer', 'pm']);
    return equal(job, 'programmer', ' is programmer');
  });

  test('lpp.getConfigVal, propName is object!', function() {
    var age, config, height, hobbies, job, name, sex, _ref;

    config = this.config;
    _ref = lpp.getConfigVal(config, {
      name: {
        defaultVal: 'DefaultVal',
        dataTypes: 'string',
        success: function(val) {
          return "hello " + val;
        }
      },
      age: {
        defaultVal: 'DefaultVal',
        dataTypes: ['int'],
        success: function(val) {
          return "My age is " + val;
        }
      },
      sex: {
        defaultVal: 'DefaultVal',
        dataTypes: 'bool',
        valRange: [true]
      },
      height: {
        defaultVal: 'DefaultVal',
        dataTypes: ['int', 'string'],
        success: function(val) {
          if (lpp.isInt(val)) {
            return "" + val + "cm";
          } else {
            return val;
          }
        }
      },
      hobbies: {
        defaultVal: null,
        dataTypes: 'Array'
      },
      job: {
        defaultVal: null,
        dataTypes: 'string',
        valRange: ['programmer', 'pm']
      }
    }), name = _ref[0], age = _ref[1], sex = _ref[2], height = _ref[3], hobbies = _ref[4], job = _ref[5];
    equal(name, 'hello John', "test property in string type");
    equal(age, 'DefaultVal', "age isn't int");
    equal(sex, true, "false stands for men!");
    equal(height, '173cm', 'testing tall');
    equal(hobbies, config.hobbies, 'is array');
    return equal(job, 'programmer', ' is programmer');
  });

  test('lpp.splice', function() {
    var result, result1, str;

    str = '0123456789abcdef';
    result = lpp.splice(str, 1, 2);
    result1 = lpp.Str.splice(str, 1, 2);
    equal(str, '0123456789abcdef', 'str');
    equal(result.items.join(''), '03456789abcdef', 'result');
    return equal(result1.str, '03456789abcdef', 'result1');
  });

  test('lpp.toArray', function() {
    var result, str;

    str = '012';
    debugger;
    result = lpp.toArray(str);
    return deepEqual(result, ["0", "1", "2"]);
  });

}).call(this);

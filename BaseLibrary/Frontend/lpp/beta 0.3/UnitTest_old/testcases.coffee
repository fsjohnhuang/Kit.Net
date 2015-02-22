module 'lpp.getConfigVal', 
	setup: ->
		@config =
			name: 'John'
			age: 21.2
			sex: true
			height: 173
			weight: 150.56
			hobbies: ['dancing', 'singing']
			job: 'programmer'

test 'lpp.getConfigVal, propName is string!', ->
	config = @config

	name = lpp.getConfigVal config, 'name', 'DefaultVal', 'string', (val)-> "hello #{val}" 
	equal  name, 'hello John', "test property in string type"

	age = lpp.getConfigVal config, 'age', 'DefaultVal', ['int'], (val)-> "My age is #{val}"
	equal age, 'DefaultVal', "age isn't int"

	sex = lpp.getConfigVal config, 'sex', 'DefaultVal', 'bool', [true]
	equal sex, true, "false stands for men!"

	height = lpp.getConfigVal config, 'height', 'DefaultVal', ['int', 'string'], (val)-> 
		if lpp.isInt val then "#{val}cm" else val
	equal height, '173cm', 'testing tall'

	hobbies = lpp.getConfigVal config, 'hobbies', null, 'Array'
	equal hobbies, config.hobbies, 'is array'

	job = lpp.getConfigVal config, 'job', null, 'string', ['programmer', 'pm']
	equal job, 'programmer', ' is programmer'


test 'lpp.getConfigVal, propName is object!', ->
	config = @config
	
	[name, age, sex, height, hobbies, job] = lpp.getConfigVal config, 
		name:
			defaultVal: 'DefaultVal'
			dataTypes: 'string'
			success: (val)-> "hello #{val}"
		age:
			defaultVal: 'DefaultVal'
			dataTypes: ['int']
			success: (val)-> "My age is #{val}"
		sex:
			defaultVal: 'DefaultVal'
			dataTypes: 'bool'
			valRange: [true]
		height:
			defaultVal: 'DefaultVal'
			dataTypes: ['int', 'string']
			success: (val)-> 
				if lpp.isInt val then "#{val}cm" else val
		hobbies:
			defaultVal: null
			dataTypes: 'Array'
		job:
			defaultVal: null
			dataTypes: 'string'
			valRange: ['programmer', 'pm']


	equal name, 'hello John', "test property in string type"
	equal age, 'DefaultVal', "age isn't int"
	equal sex, true, "false stands for men!"
	equal height, '173cm', 'testing tall'
	equal hobbies, config.hobbies, 'is array'
	equal job, 'programmer', ' is programmer'

test 'lpp.splice', ->
	str = '0123456789abcdef'
	result = lpp.splice(str, 1, 2)
	result1 = lpp.Str.splice str, 1, 2
	
	equal str, '0123456789abcdef', 'str'
	
	equal result.items.join(''), '03456789abcdef', 'result'
	equal result1.str, '03456789abcdef', 'result1'

test 'lpp.toArray', ->
	str = '012'
	debugger
	result = lpp.toArray str
	deepEqual result, ["0","1","2"]
### 语法树元素
type: 类型
	0: 普通文本 StrEl
	1: 变量 VarEl
	2: 方法 FnEl
	3: 条件语句 IfEl
	4: 循环语句 LoopEl
	5：指令, break、continue CmdEl
	6: 语句块 @{ ...... } BlockEl
	7: 区间 @[1..3] 或 @[1...3] SectionEl
condition: 对于条件语句和循环语句使用的执行条件
scope: 作用域
###
class SyntaxEl
	constructor: (@content, @type, @condition, @scope, @childEl) ->


class GP
	
	_keyword = ['if', 'elseif', 'elif', 'else', 'unless', 'while', 'for', 'true', 'false', 'null', 'break', 'continue'] # 关键字
	_relSymbol = ['&&', '||'] # 关系运算符
	_logicSymbol = ['>', '>=', '<', '<=', '==', '!='] # 逻辑运算符
	_mathSymbol = ['+', '-', '*', '/', '%', '**'] # 数学运算符
	_gpSymbol = ['<-'] # 赋值操作符

	_symbolStart = ///[_a-Z[{]'///
	_symbolEnd = ///\}\]///

	# 标识符命名规则
	_symbolExp = '[_a-Z][_0-9a-Z]'

	_tpl = ''
	_prefix = '@'

	# tpl 模板文本
	constructor: (tpl, prefix) ->
		_tpl = tpl
		_prefix = prefix if prefix?


	_root = lpp.create('lpp.structure.TreeNode') # 语法书根节点
	_spider = # 文本蜘蛛
		i: 0 # 当前索引
		g: null # 当前字符
		ng: null # 下一个字符
		el: null # 当前语法元素
		pEl: _root # 当前父语法元素
		'{': 0
		'[': 0
		parse: () ->
			

	# 对模板文本进行语法分析并生成语法树
	_compile = () ->
		grains = lpp.toArray(_tpl)
		grainsLen = grains.length

		for i, grain in grains
			_spider.i = i
			_spider.g = grain
			_spider.ng = if i < grainsLen - 1 then grains[i + 1] else null




lpp.define 'lpp.tpl', GP
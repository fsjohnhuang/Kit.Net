/* 语法树元素
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
*/


(function() {
  var GP, SyntaxEl;

  SyntaxEl = (function() {
    function SyntaxEl(content, type, condition, scope, childEl) {
      this.content = content;
      this.type = type;
      this.condition = condition;
      this.scope = scope;
      this.childEl = childEl;
    }

    return SyntaxEl;

  })();

  GP = (function() {
    var _compile, _gpSymbol, _keyword, _logicSymbol, _mathSymbol, _prefix, _relSymbol, _root, _spider, _symbolEnd, _symbolExp, _symbolStart, _tpl;

    _keyword = ['if', 'elseif', 'elif', 'else', 'unless', 'while', 'for', 'true', 'false', 'null', 'break', 'continue'];

    _relSymbol = ['&&', '||'];

    _logicSymbol = ['>', '>=', '<', '<=', '==', '!='];

    _mathSymbol = ['+', '-', '*', '/', '%', '**'];

    _gpSymbol = ['<-'];

    _symbolStart = /[_a-Z[{]'/;

    _symbolEnd = /\}\]/;

    _symbolExp = '[_a-Z][_0-9a-Z]';

    _tpl = '';

    _prefix = '@';

    function GP(tpl, prefix) {
      _tpl = tpl;
      if (prefix != null) {
        _prefix = prefix;
      }
    }

    _root = lpp.create('lpp.structure.TreeNode');

    _spider = {
      i: 0,
      g: null,
      ng: null,
      el: null,
      pEl: _root,
      '{': 0,
      '[': 0,
      parse: function() {}
    };

    _compile = function() {
      var grain, grains, grainsLen, i, _i, _len, _results;

      grains = lpp.toArray(_tpl);
      grainsLen = grains.length;
      _results = [];
      for (grain = _i = 0, _len = grains.length; _i < _len; grain = ++_i) {
        i = grains[grain];
        _spider.i = i;
        _spider.g = grain;
        _results.push(_spider.ng = i < grainsLen - 1 ? grains[i + 1] : null);
      }
      return _results;
    };

    return GP;

  })();

  lpp.define('lpp.tpl', GP);

}).call(this);

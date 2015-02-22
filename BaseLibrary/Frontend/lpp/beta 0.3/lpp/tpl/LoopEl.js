(function() {
  var LoopEl;

  LoopEl = (function() {
    function LoopEl(parentNode, children, ctx, scope, condition) {
      var p, v, _ref;

      this.condition = condition;
      LoopEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 4, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return LoopEl;

  })();

  lpp.define('lpp.tpl.LoopEl', LoopEl, 'lpp.structure.TreeNode');

}).call(this);

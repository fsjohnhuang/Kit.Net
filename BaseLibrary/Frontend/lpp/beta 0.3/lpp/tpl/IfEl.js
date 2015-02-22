(function() {
  var IfEl;

  IfEl = (function() {
    function IfEl(parentNode, children, ctx, scope, condition) {
      var p, v, _ref;

      this.condition = condition;
      IfEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 3, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return IfEl;

  })();

  lpp.define('lpp.tpl.IfEl', IfEl, 'lpp.structure.TreeNode');

}).call(this);

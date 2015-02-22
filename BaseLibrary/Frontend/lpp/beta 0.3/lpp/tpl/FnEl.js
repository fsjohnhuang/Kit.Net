(function() {
  var FnEl;

  FnEl = (function() {
    function FnEl(parentNode, children, ctx, scope, fnName, args) {
      var p, v, _ref;

      this.fnName = fnName != null ? fnName : '';
      this.args = args != null ? args : [];
      FnEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 2, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return FnEl;

  })();

  lpp.define('lpp.tpl.FnEl', FnEl, 'lpp.structure.TreeNode');

}).call(this);

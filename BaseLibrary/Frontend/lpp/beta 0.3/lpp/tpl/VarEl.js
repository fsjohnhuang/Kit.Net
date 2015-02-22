(function() {
  var VarEl;

  VarEl = (function() {
    function VarEl(parentNode, children, ctx, scope, varName) {
      var p, v, _ref;

      this.varName = varName != null ? varName : '';
      VarEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 1, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return VarEl;

  })();

  lpp.define('lpp.tpl.VarEl', VarEl, 'lpp.structure.TreeNode');

}).call(this);

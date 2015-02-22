(function() {
  var BlockEl;

  BlockEl = (function() {
    function BlockEl(parentNode, children, ctx, scope) {
      var p, v, _ref;

      BlockEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 6, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return BlockEl;

  })();

  lpp.define('lpp.tpl.BlockEl', BlockEl, 'lpp.structure.TreeNode');

}).call(this);

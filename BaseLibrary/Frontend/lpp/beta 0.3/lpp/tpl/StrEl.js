(function() {
  var StrEl;

  StrEl = (function() {
    function StrEl(parentNode, children, ctx, scope, content) {
      var p, v, _ref;

      this.content = content != null ? content : '';
      StrEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 0, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return StrEl;

  })();

  lpp.define('lpp.tpl.StrEl', StrEl, 'lpp.structure.TreeNode');

}).call(this);

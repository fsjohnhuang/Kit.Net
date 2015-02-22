(function() {
  var IfEntryEl;

  IfEntryEl = (function() {
    function IfEntryEl(parentNode, children, ctx, scope) {
      var p, v, _ref;

      IfEntryEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 3, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return IfEntryEl;

  })();

  lpp.define('lpp.tpl.IfEntryEl', IfEntryEl, 'lpp.structure.TreeNode');

}).call(this);

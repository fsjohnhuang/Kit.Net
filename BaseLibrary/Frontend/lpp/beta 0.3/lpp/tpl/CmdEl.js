(function() {
  var CmdEl;

  CmdEl = (function() {
    function CmdEl(parentNode, children, ctx, scope, cmd) {
      var p, v, _ref;

      this.cmd = cmd;
      CmdEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 5, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return CmdEl;

  })();

  lpp.define('lpp.tpl.CmdEl', CmdEl, 'lpp.structure.TreeNode');

}).call(this);

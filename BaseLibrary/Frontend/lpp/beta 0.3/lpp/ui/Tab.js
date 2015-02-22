/*
tab组件
*/


(function() {
  var Tab;

  Tab = (function() {
    function Tab(config) {
      this.id = lpp.getConfigVal(config, 'id', 'tab' + (+(new Date())), 'string');
      this.titleCls = lpp.getConfigVal(config, 'titleCls', '', 'string');
      this.curTabCls = lpp.getConfigVal(config, 'curTabCls', '', 'string');
      this.separatorCls = lpp.getConfigVal(config, 'separatorCls', '', 'string');
      this.separatorTpl = lpp.getConfigVal(config, 'separatorTpl', '', 'string');
      this.tabTpl = lpp.getConfigVal(config, 'tabTpl', '', 'string');
      this.listCls = lpp.getConfigVal(config, 'listCls', '', 'string');
      this.itemTpl = lpp.getConfigVal(config, 'itemTpl', '', 'string');
    }

    Tab.prototype.bind = function(ds, ctn) {
      var i, item, itemData, itemHtml, j, list, listData, listDatas, lists, separatorHtml, tab, tabCtn, tabData, tabDatas, tabHtml, _i, _itemTpl, _j, _k, _l, _len, _len1, _len2, _len3;

      if (lpp.isStr(ds)) {
        lpp.Ajax.syncReq(ds, function(xhr, state) {
          return ds = JSON.parse(xhr.responseText);
        });
      }
      tabDatas = [];
      listDatas = [];
      for (_i = 0, _len = ds.length; _i < _len; _i++) {
        item = ds[_i];
        tabDatas.push(item.tab);
        listDatas.push(item.items);
      }
      tabCtn = lpp(lpp.Str.format('<ul {0}></ul>', (lpp.isEmpty(this.titleCls) ? '' : 'class="' + this.titleCls + '"')));
      for (i = _j = 0, _len1 = tabDatas.length; _j < _len1; i = ++_j) {
        tabData = tabDatas[i];
        separatorHtml = i !== 0 ? lpp.Str.format('<li {0}>{1}</li>', (lpp.isEmpty(this.separatorCls) ? '' : 'class="' + this.separatorCls + '"'), this.separatorTpl) : '';
        tabHtml = lpp.Str.format('<li id="{0}" {1}></li>', 'tab' + this.id + '_t' + i, (i === 0 ? (lpp.isEmpty(this.curTabCls) ? '' : 'class="' + this.curTabCls + '"') : ''));
        tab = lpp(tabHtml);
        if (!lpp.isEmpty(this.tabTpl)) {
          tab.append(lpp.Str.tpl(this.tabTpl, tabData));
          tab.on('mouseover', (function(id, i, len, curTabCls) {
            return function(e) {
              var curList, curTab, j, _k, _results;

              _results = [];
              for (j = _k = 0; 0 <= len ? _k < len : _k > len; j = 0 <= len ? ++_k : --_k) {
                curTab = lpp('#tab' + id + '_t' + j);
                curList = lpp('#' + id + '_l' + j);
                if (j === i) {
                  curList.show();
                  _results.push(curTab.addCls(curTabCls));
                } else {
                  curList.hide();
                  _results.push(curTab.removeCls(curTabCls));
                }
              }
              return _results;
            };
          })(this.id, i, tabDatas.length, this.curTabCls));
        }
        if (!lpp.isEmpty(separatorHtml)) {
          tabCtn.append(lpp(separatorHtml));
        }
        tabCtn.append(tab);
      }
      lists = [];
      _itemTpl = lpp.Str.format('<li>{0}</li>', this.itemTpl);
      for (i = _k = 0, _len2 = listDatas.length; _k < _len2; i = ++_k) {
        listData = listDatas[i];
        list = lpp(lpp.Str.format('<ul {0} {1} id="{2}"></ul>', (lpp.isEmpty(this.listCls) ? '' : lpp.Str.format('class="{0}"', this.listCls)), (i === 0 ? '' : 'style="display:none;"'), this.id + '_l' + i));
        lists.push(list);
        for (j = _l = 0, _len3 = listData.length; _l < _len3; j = ++_l) {
          itemData = listData[j];
          itemHtml = lpp.Str.tpl(_itemTpl, itemData);
          list.append(lpp(itemHtml));
        }
      }
      return lpp(ctn).append(tabCtn).back().append(lists);
    };

    return Tab;

  })();

  lpp.define('lpp.ui.Tab', Tab);

}).call(this);

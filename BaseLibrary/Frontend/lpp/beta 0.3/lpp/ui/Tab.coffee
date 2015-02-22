###
tab组件
###
class Tab

	constructor: (config) ->
		@id = lpp.getConfigVal(config, 'id', 'tab' + (+(new Date())), 'string')
		@titleCls = lpp.getConfigVal(config, 'titleCls', '', 'string')
		@curTabCls = lpp.getConfigVal(config, 'curTabCls', '', 'string')
		@separatorCls = lpp.getConfigVal(config, 'separatorCls', '', 'string')
		@separatorTpl = lpp.getConfigVal(config, 'separatorTpl', '', 'string')
		@tabTpl = lpp.getConfigVal(config, 'tabTpl', '', 'string')

		@listCls = lpp.getConfigVal(config, 'listCls', '', 'string')
		@itemTpl = lpp.getConfigVal(config, 'itemTpl', '', 'string')

	bind: (ds, ctn) ->
		# ds为url时，发起数据请求
		if lpp.isStr ds
			lpp.Ajax.syncReq ds, (xhr, state)->
				ds = JSON.parse xhr.responseText

		tabDatas = []
		listDatas = [];
		for item in ds
			tabDatas.push item.tab
			listDatas.push item.items

		# 生成tab
		tabCtn = lpp lpp.Str.format('<ul {0}></ul>', (if lpp.isEmpty @titleCls then '' else 'class="' + @titleCls + '"'))
		for tabData ,i in tabDatas
			separatorHtml = if i isnt 0 then lpp.Str.format('<li {0}>{1}</li>', (if lpp.isEmpty @separatorCls then '' else 'class="' + @separatorCls + '"'), @separatorTpl) else ''
			tabHtml = lpp.Str.format('<li id="{0}" {1}></li>', 
				'tab' + @id + '_t' + i,
				(if i == 0 then (if lpp.isEmpty @curTabCls then '' else 'class="' + @curTabCls + '"') else ''))
			tab = lpp tabHtml
			unless lpp.isEmpty @tabTpl
				tab.append lpp.Str.tpl(@tabTpl, tabData) 
				tab.on 'mouseover', do (id = @id, i, len = tabDatas.length, curTabCls = @curTabCls)->
					(e) ->
						for j in [0...len]
							curTab = lpp('#tab' + id + '_t' + j)
							curList = lpp('#' + id + '_l' + j)
							if j == i
								curList.show()
								curTab.addCls curTabCls
							else
								curList.hide()
								curTab.removeCls curTabCls
			tabCtn.append lpp(separatorHtml) unless lpp.isEmpty separatorHtml
			tabCtn.append tab

		# 生成list
		lists = []
		_itemTpl = lpp.Str.format '<li>{0}</li>', @itemTpl
		for listData, i in listDatas
			list = lpp lpp.Str.format('<ul {0} {1} id="{2}"></ul>',
				(if lpp.isEmpty @listCls then '' else lpp.Str.format('class="{0}"', @listCls)), 
				(if i == 0 then '' else 'style="display:none;"'),
				@id + '_l' + i)
			lists.push list
			for itemData, j in listData
				itemHtml = lpp.Str.tpl _itemTpl, itemData
				list.append lpp(itemHtml)
		
		lpp(ctn).append(tabCtn).back().append(lists)
		

lpp.define 'lpp.ui.Tab', Tab
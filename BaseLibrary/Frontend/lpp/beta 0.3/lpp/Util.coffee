@lpp.Util = 
	addFavorites: ->
		try
			window.external.AddFavorite location.href, document.title
		catch e
			try
				window.sidebar.addPanel document.title, location.href, ''
			catch ex
				alert '加入收藏失败，请使用Ctrl+D进行添加'
	setHome: ->
		e = event || arguments[0]
		target = e.target || e.srcElement
		try
			target.style.behavior = 'url(#default#homepage)'
			target.setHomePage location.href
		catch e
			if window.netscape?
				try
					netscape.security.PrivilegeManager.enablePrivilege 'UniversalXPConnect'
				catch ex
					alert "此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。"
					prefs = Components.classes['@mozilla.org/preferences-service;1'].getService Components.interfaces.nsIPrefBranch
				prefs.setCharPref 'browser.startup.homepage', location.href
			else
				alert "此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。"
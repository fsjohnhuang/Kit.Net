@lpp.Util = 
	addFavorites: ->
		try
			window.external.AddFavorite location.href, document.title
		catch e
			try
				window.sidebar.addPanel document.title, location.href, ''
			catch ex
				alert '�����ղ�ʧ�ܣ���ʹ��Ctrl+D�������'
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
					alert "�˲�����������ܾ���\n�����������ַ�����롰about:config�����س�\nȻ�� [signed.applets.codebase_principal_support]��ֵ����Ϊ'true',˫�����ɡ�"
					prefs = Components.classes['@mozilla.org/preferences-service;1'].getService Components.interfaces.nsIPrefBranch
				prefs.setCharPref 'browser.startup.homepage', location.href
			else
				alert "�˲�����������ܾ���\n�����������ַ�����롰about:config�����س�\nȻ�� [signed.applets.codebase_principal_support]��ֵ����Ϊ'true',˫�����ɡ�"
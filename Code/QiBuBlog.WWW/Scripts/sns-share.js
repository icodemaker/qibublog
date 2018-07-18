!function($) { "use strict";

var snsShare = {
	sinawb : function(title, href) {
		return 'http://v.t.sina.com.cn/share/share.php' +
            '?title=' + encodeURIComponent(title) + '&url=' + encodeURIComponent(href);
	},
	
	tencentwb : function(title, href) {
		return 'http://v.t.qq.com/share/share.php' +
			'?title=' + encodeURIComponent(title) + '&url=' + encodeURIComponent(href);
	},
	
	qqspace : function(title, href) {
		return 'http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey' +
			'?title=' + encodeURIComponent(title) + '&url=' + encodeURIComponent(href);
	},

    neteasewb : function(title, href) {
        return 'http://t.163.com/article/user/checkLogin.do?' +
            '?info=' + encodeURIComponent(title) + '&link=' + encodeURIComponent(href);
    },

    sohuwb : function(title, href) {
        return 'http://t.sohu.com/third/post.jsp?' +
            '?title=' + encodeURIComponent(title) + '&url=' + encodeURIComponent(href);
    },
	
	renren : function(title, href) {
		return 'http://share.renren.com/share/buttonshare.do' +
			'?title=' + encodeURIComponent(title) + '&link=' + encodeURIComponent(href);
	},
	
	douban : function(title, href) {
		return 'http://www.douban.com/recommend/' +
			'?title=' + encodeURIComponent(title) + '&url=' + encodeURIComponent(href);
	},

    googleplus : function(title, href) {
        return 'https://plus.google.com/share' + 
            '?t=' + encodeURIComponent(title) + '&url=' + encodeURIComponent(href);
    }
};

var title = $('title').html();
$('a[data-snsname]').each(function(i, a) {
    this.target = '_blank';
    this.href = snsShare[this.getAttribute('data-snsname')](title, location.href);
});

}(jQuery);
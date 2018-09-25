!function (a) {
    "use strict";
    var b = {
        url: "/Comment/List",
        wrapper: a("#comment-list > div"),
        articleId: a("#comment-articleid").val(),
        listTemplate: '<# for (var i = 0; i < data.length; i++) { #><article<# if (data[i].AuthorId > 0) { #> class="comment-by-blogger"<# } #>>    <header>        <address class="comment-author">    <# if (data[i].HomePage) { #>        <a href="<#=data[i].HomePage #>" target="_blank"><#=data[i].AuthorName #></a>    <# } else { #>        <#=data[i].AuthorName #>    <# } #></address> 在 <time><#=dateUtility.iFormat(dateUtility.parseCSharpJSONDate(data[i].PostTime)) #></time> 说：    </header>    <div class="comment-content"><#=httpUtility.htmlEncode(data[i].Content).replace(/\\r\\n/g, "<br />") #></div></article><# } #><div class="pager"></div>',
        tipsTemplate: '<p class="comment-tips"><#=tips#></p>',
        init: function () {
            this.articleId && this.load(1);
        },
        load: function (b, c) {
            var d = this;
            this.wrapper.empty().html(a.ui.parseTpl(this.tipsTemplate, {
                tips: "正在读取评论，请稍候..."
            })), a.ajax({
                url: this.url,
                dataType: "json",
                data: {
                    articleId: this.articleId,
                    page: b
                },
                success: function (a) {
                    d.render(a, c);
                }
            })
        },
        render: function (c, d) {
            c.Data && c.Data.length
                ? (this.wrapper.html(a.ui.parseTpl(this.listTemplate,
                    {
                        data: c.Data
                    })), a("div.pager", this.wrapper).pager({
                        pageCount: c.PageCount,
                        currentPage: c.CurrentPage,
                        hrefTpl: "#comment-<#=page#>",
                        onclick: function (a) {
                            /comment-(\d+)$/.test(this.href) && (a.preventDefault(), b.load(RegExp.$1, true));
                        }
                    }))
                : this.wrapper.html(a.ui.parseTpl(this.tipsTemplate,
                    {
                        tips: "暂无评论"
                    })), d && (window.location.hash = "comment-list");
        }
    };
    b.init();
    var c = new Captcha(a("div.captcha")),
        d = parseInt(a("#comment-minlength").val()),
        e = parseInt(a("#comment-maxlength").val()),
        f = [{
            fields: "Content",
            errorText: "评论内容不能少于" + d + "个字",
            rules: function (a) {
                return a.length >= d;
            }
        }, {
            fields: "Content",
            errorText: "评论内容不能多于" + e + "个字",
            required: false,
            rules: function (a) {
                return a.length <= e;
            }
        }];
    a("p.comment-user").length ||
        f.push({
            fields: "Email",
            rules: "isemail",
            errorText: "Email格式错误",
            required: false
        },
            {
                fields: "AuthorName",
                errorText: "请填写您的称呼"
            },
            {
                fields: "captcha",
                errorText: "请填写验证码"
            }), a("div.comment-form form").validate({
                flow: f,
                submit: function (d, e, f) {
                    e.preventDefault();
                    var g = a("input[type=submit]", d);
                    if (g.attr("disabled")) {
                        alert("正在提交评论，请稍后...");
                        return;
                    }
                    g.val("发表中，请稍后...").attr("disabled", true), a.ajax({
                        url: d.action,
                        type: "POST",
                        dataType: "json",
                        data: f,
                        success: function (e) {
                            e.Message && alert(e.Message), e.hasOwnProperty("CommentData") &&
                                (e.CommentData != null && b.render(e.CommentData, true), a(d).trigger("reset")),
                                c.refresh();
                        },
                        complete: function () {
                            g.val("发 表").attr("disabled", !1);
                        }
                    });
                }
            }).bind("reset",
                function () {
                    var b = this;
                    setTimeout(function () {
                        a("[name]", b).blur();
                    },
                        10);
                });
}(jQuery)
!function() {

$("input[type=reset]").click(function(e) {
    if (this.getAttribute("data-isclear")) {
        e.preventDefault();

        var form = this.form;
        if (!form) { return; }

        for (var i = 0; i < form.length; i++) {
            if (form[i].name) { form[i].value = ''; };
        }

        this.removeAttribute("data-isclear");

        this.value = "恢 复";
    } else {
        this.setAttribute("data-isclear", "true");

        this.value = "清 空";
    }
});

}();
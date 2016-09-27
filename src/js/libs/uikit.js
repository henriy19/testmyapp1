(function () {
    var uikit = { version: '1.0.0' };

    uikit.polling = function (selector, option) {
        var tmpl = [
            '<div><p>{{text}}</p></div>',
            '<div>',
            '<select>',
            '{{#each data}}',
            '<option value="{{id}}">{{text}}</option>',
            '{{/each}}',
            '</select>',
            '<button data-action="send">Send</button>',
            '</div>',
        ].join('');

        var html = Handlebars.compile(tmpl)(option);
        document.querySelector(selector).innerHTML = html;

        if (option.onSend) {
            $(selector + ' [data-action=send]').on('click', function (e) {
                e.preventDefault();
                option.onSend({value:'click', text: new Date()});
            })
        }
    };

    window.uikit = uikit;
} ());
(function () {
    console.log('test 1212');

    initTimeline();


    function initTimeline() {
        $.post('twitter/GetProfile', function (r) {
            if (r.success) {
                var data = JSON.parse(r.data);
                console.log(data[0]);

                var tmpl = [
                    '{{#each this}}',
                    '<div class="timeline-item">',
                    '<div>id : {{id_str}}</div>',
                    '<div>Tweet Data Time : {{created_at}}</div>',
                    '<div>Tweet : {{text}}</div>',
                    '<div>Tweetby : {{user.name}}</div>',
                    '<div>Followers : {{followers_count}}</div>',
                    '<div class="tb-images">',
                    '<img src="images/reply_tweet.png" alt="Submit" data-action="reply">',
                    '<img src="images/retweet.png" data-id="{{id_str}}" alt="Submit" data-action="retweet">',
                    '<img src="images/heart.png" alt="Submit" data-action="like">',
                    '</div>',
                    '</div>',
                    '{{/each}}',
                ].join('');

                var html = Handlebars.compile(tmpl)(data);
                $('[data-name=tw-timeline]').html(html);

                $('[data-name=tw-timeline] .timeline-item [data-action="retweet"]').on('click', function () {
                    var _this = $(this);
                    console.log(_this.data().id);
                });
            };
        });

    }
} ());
$(document).ready(function () {
	$('#allEmpl').click(function(){
		$('#singleEmployee').hide();
		$('#allEmployee').show();
	});
	
	$('#singEmpl').click(function(){
		$('#allEmployee').hide();
		$('#singleEmployee').show();
	});	
	
	
	$('#allTasks').click(function(){
		$('#singleT').hide();
		$('#allT').show();
	});
	
	$('#singleTask').click(function(){
		$('#allT').hide();
		$('#singleT').show();
    });


    var th = jQuery('th'), inverse = false;
    th.click(function () {
        var header = $(this), index = header.index();

        header
            .closest('table')
            .find('td')
            .filter(function () {
                return $(this).index() === index;
            })
            .sort(function (a, b) {

                a = $(a).text();
                b = $(b).text();

                return (
                    isNaN(a) || isNaN(b) ?
                        a > b : +a > +b
                    ) ?
                        inverse ? -1 : 1 :
                        inverse ? 1 : -1;

            }, function () {
                return this.parentNode;
            });

    inverse = !inverse;

});
});

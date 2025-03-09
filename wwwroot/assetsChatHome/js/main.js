$(document).ready(function () {
	"use strict"; // start of use strict

	/*==============================
	Mobile navigation
	==============================*/
	$('.header__btn').on('click', function() {
		$(this).toggleClass('active');
		$('.header__nav').toggleClass('active');
	});

	/*==============================
	Filter
	==============================*/
	$('.filter__search input').on('click', function () {
		$('.filter__search').toggleClass('focus');
	});
	$(document).on('click', function (e) {
		if (!$(e.target).closest('.filter__search.focus').length) {
			$('.filter__search').removeClass('focus');
		}
		e.stopPropagation();
	});

	/*==============================
	Home slider
	==============================*/
	$('.home-carousel').owlCarousel({
		animateOut: 'fadeOut',
		animateIn: 'fadeIn',
		mouseDrag: false,
		dots: false,
		items: 1,
		loop: true,
		autoplay: true,
		autoplayTimeout: 5000,
		smartSpeed: 600,
	});

	$('.slideshow-item, .article-card--bg, .sign').each(function () {
		if ($(this).attr("data-bg")){
			$(this).css({
				'background': 'url(' + $(this).data('bg') + ')',
				'background-position': 'center center',
				'background-repeat': 'no-repeat',
				'background-size': 'cover'
			});
		}
	});

	/*==============================
	Card slider
	==============================*/
	$('.card-carousel').owlCarousel({
		loop: true,
		autoplay: true,
		autoplayTimeout: 6000,
		autoplayHoverPause: true,
		smartSpeed: 600,
		autoHeight: true,
		responsive:{
			0:{
				items:1
			},
			360:{
				items:1
			},
			768:{
				items:2
			},
			992:{
				items:3
			},
			1200:{
				items:4
			}
		}
	});

	/*==============================
	Img slider
	==============================*/
	$('.img-carousel').owlCarousel({
		items: 1,
		loop: true,
		autoplay: true,
		autoplayTimeout: 6000,
		smartSpeed: 600,
	});

	/*==============================
	Back to top
	==============================*/
	$('.footer__btn').on('click', function() {
		$('body,html').animate({
			scrollTop: 0,
			}, 700 // - duration of the top scrolling animation (in ms)
		);
	});

	/*==============================
	Chat
	==============================*/
	$('.chat-button').on('click', function(e) {
		$('.chat-button--fixed').toggleClass('active');
		$('.chat').toggleClass('active');
		e.preventDefault();
	});
});
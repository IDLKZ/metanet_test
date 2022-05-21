function initSwiper() {


    const swiper = new Swiper('.swiper', {
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
            clickable:true,
        },
        slidesPerView: 1,
        spaceBetween: 20,
        slidesPerGroup: 1,
        loop: true,
        centeredSlides: true,
        breakpoints: {
            100: {
                slidesPerView: 1,
                spaceBetween: 10,
            },
            650: {
                slidesPerView: 2,
                spaceBetween: 10,
            },
            
            900: {
                slidesPerView: 3,
                spaceBetween: 5,
            },
            1100:{
                slidesPerView: 4,
                spaceBetween: 5,
            }
        }
    });
}
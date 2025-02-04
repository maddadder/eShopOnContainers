import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { BasketService } from './basket.service';
import { IBasket } from '../shared/models/basket.model';
import { IBasketItem } from '../shared/models/basketItem.model';
import { BasketWrapperService } from '../shared/services/basket.wrapper.service';
import { ConfigurationService } from '../shared/services/configuration.service';

@Component({
    selector: 'esh-basket .esh-basket .mb-5',
    styleUrls: ['./basket.component.scss'],
    templateUrl: './basket.component.html'
})
export class BasketComponent implements OnInit {
    errorMessages: any;
    basket: IBasket;
    totalPrice: number = 0;

    constructor(private configurationService: ConfigurationService, private basketSerive: BasketService, private router: Router, private basketWrapperService: BasketWrapperService) { }

    ngOnInit() {
        if (this.configurationService.isReady) {
            this.loadBasketData();
        } else {
            // Subscribe to the settingsLoaded$ observable to know when settings are ready
            this.configurationService.settingsLoaded$.subscribe(() => {
                this.loadBasketData();
            });
        }
    }
    
    private loadBasketData() {
        this.basketSerive.getBasket().subscribe(basket => {
            this.basket = basket;
            this.calculateTotalPrice();
        });
    }

    deleteItem(id: String) {
        this.basket.items = this.basket.items.filter(item => item.id !== id);
        this.calculateTotalPrice();
        
        this.basketSerive.setBasket(this.basket).subscribe(x => 
            {
                this.basketSerive.updateQuantity();
                console.log('basket updated: ' + x)
            }
        );
    }

    itemQuantityChanged(item: IBasketItem, quantity: number) {
        item.quantity = quantity > 0 ? quantity : 1;
        this.calculateTotalPrice();
        this.basketSerive.setBasket(this.basket).subscribe(x => console.log('basket updated: ' + x));
    }

    update(event: any): Observable<boolean> {
        let setBasketObservable = this.basketSerive.setBasket(this.basket);
        setBasketObservable
            .subscribe(
            x => {
                this.errorMessages = [];
                console.log('basket updated: ' + x);
            },
            errMessage => this.errorMessages = errMessage.messages);
        return setBasketObservable;
    }

    checkOut(event: any) {
        this.update(event)
            .subscribe(
                x => {
                    this.errorMessages = [];
                    this.basketWrapperService.basket = this.basket;
                    this.router.navigate(['order']);
        });
    }

    private calculateTotalPrice() {
        this.totalPrice = 0;
        this.basket.items.forEach(item => {
            this.totalPrice += (item.unitPrice * item.quantity);
        });
    }
}

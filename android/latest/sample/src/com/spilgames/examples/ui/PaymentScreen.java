package com.spilgames.examples.ui;

import android.app.Activity;
import android.os.Bundle;

import com.spilgames.examples.R;
import com.spilgames.framework.Spil;
import com.spilgames.framework.SpilInterface;
import com.spilgames.framework.payments.impl.SpilPayments;

public class PaymentScreen extends Activity {

	SpilPayments spilPayments;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.payment_screen);

		SpilInterface spil = Spil.getInstance();
		spil.showStore();
		
	}

//	@Override
//	public void paymentSetUpReady() {
//		System.out.println("PAYMENTS READY");
//		List<String> skuList = new ArrayList<String>();
//		skuList.add("test_consumable_1");
//		skuList.add("test_managed_2");
//		spilPayments.paymentRequestInventory(skuList);
////		PaymentProduct product = new PaymentProduct();
////		product.setSku("test_managed_2");
////		spilPayments.paymentsStartFlow(this, product);
//
//	}
//
//	@Override
//	public void paymentSetUpFailed(String error) {
//		System.out.println("PAYMENTS FAILED");
//
//	}
//
//	@Override
//	public void paymentRecievedInventory(List<PaymentProduct> productsList) {
//		System.out.println("PRODUCT LIST "+productsList.size());
//		System.out.println("PROCUT 1 "+productsList.get(0).toString());
//		System.out.println("PROCUT 2 "+productsList.get(1).toString());
//	}
//
//	@Override
//	public void paymentFailedInventory(String error) {
//		System.out.println("INVENTORY FAILED");
//	}
//
//	@Override
//	public void paymentTransactionSucceed(String transactionId) {
//		System.out.println("Payment Success "+transactionId);
//
//	}
//
//	@Override
//	public void paymentTransactionFailed(String errorMessage) {
//		System.out.println("Payment failed "+errorMessage);
//
//	}

}

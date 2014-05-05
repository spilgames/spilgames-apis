package com.spilgames.examples.ui;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.onepf.oms.OpenIabHelper;

import android.app.Activity;
import android.os.Bundle;

import com.spilgames.examples.R;
import com.spilgames.framework.payments.SpilPaymentsCallback;
import com.spilgames.framework.payments.impl.PaymentProduct;
import com.spilgames.framework.payments.impl.SpilPayments;

public class PaymentScreen extends Activity implements SpilPaymentsCallback {

	OpenIabHelper mHelper;
	SpilPayments spilPayments;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.payment_screen);

		String googlePlayKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAj6XHgSJYzpKAOenYMiWAbcDVUFplF6Lc/bs/nYWE3R9eOk7VgiedoXtVOphkglbHVkQP8k6SgEa5xgh5xiVrGGsQcF21Iprp1owuA89NAFD1hpugAECewgaO7PlnpdOlCjdTX7yaiiDGvxsFr7FhRLo5ZN8q2khim25NR//Re71ixDh+sBypYjt/G7/B6rLbMdTMSQsHfo5D1pxi7iQjJazAwZhPYrciOVBe8q849nSLhp3oXmd0G6+W45FUVfBZQm7nhTzJt8LLh4cltzYlocnLe10ZCkbVuh6pI/OVw8OzJfjk+pJXhhr2qsLYsTX1H4lXEbK8nuI7JY3Q0i8ZIwIDAQAB/fWGS9G5UKalcWvH4il7bniwWFtLED1Zs/Y8QV8BvDrQQY2MXfByXoDQ43L73jxGhTAJFOe/bUbga1eSKN69HLa2nWBl9NknrJvjU46zGy0Rgd0u+fxqbUgIBSgMz0KhLbAqLdPjzFWlBgyDQ4rxLTVXuOeRsUA8xa3DYhYNlhV1S6yml5RqRNYTSgwOch1JFyxphBtn4/E4jBtYjruqpG6prxzMWeVwIDAQAB/bs/nYWE3R9eOk7VgiedoXtVOphkglbHVkQP8k6SgEa5xgh5xiVrGGsQcF21Iprp1owuA89NAFD1hpugAECewgaO7PlnpdOlCjdTX7yaiiDGvxsFr7FhRLo5ZN8q2khim25NR//Re71ixDh+sBypYjt/G7/B6rLbMdTMSQsHfo5D1pxi7iQjJazAwZhPYrciOVBe8q849nSLhp3oXmd0G6+W45FUVfBZQm7nhTzJt8LLh4cltzYlocnLe10ZCkbVuh6pI/OVw8OzJfjk+pJXhhr2qsLYsTX1H4lXEbK8nuI7JY3Q0i8ZIwIDAQAB/fWGS9G5UKalcWvH4il7bniwWFtLED1Zs/Y8QV8BvDrQQY2MXfByXoDQ43L73jxGhTAJFOe/bUbga1eSKN69HLa2nWBl9NknrJvjU46zGy0Rgd0u+fxqbUgIBSgMz0KhLbAqLdPjzFWlBgyDQ4rxLTVXuOeRsUA8xa3DYhYNlhV1S6yml5RqRNYTSgwOch1JFyxphBtn4/E4jBtYjruqpG6prxzMWeVwIDAQAB";
		Map<String, String> storeKeys = new HashMap<String, String>();
		storeKeys.put(OpenIabHelper.NAME_GOOGLE, googlePlayKey);
		mHelper = new OpenIabHelper(this, storeKeys);
		spilPayments = new SpilPayments(mHelper);
		spilPayments.setPaymentListener(this);
		spilPayments.setUpPayments();
	}

	@Override
	public void paymentSetUpReady() {
		System.out.println("PAYMENTS READY");
		List<String> skuList = new ArrayList<String>();
		skuList.add("test_consumable_1");
		skuList.add("test_managed_2");
		spilPayments.paymentRequestInventory(skuList);
//		PaymentProduct product = new PaymentProduct();
//		product.setSku("test_managed_2");
//		spilPayments.paymentsStartFlow(this, product);

	}

	@Override
	public void paymentSetUpFailed(String error) {
		System.out.println("PAYMENTS FAILED");

	}

	@Override
	public void paymentRecievedInventory(List<PaymentProduct> productsList) {
		System.out.println("PRODUCT LIST "+productsList.size());
		System.out.println("PROCUT 1 "+productsList.get(0).toString());
		System.out.println("PROCUT 2 "+productsList.get(1).toString());
	}

	@Override
	public void paymentFailedInventory(String error) {
		System.out.println("INVENTORY FAILED");
	}

	@Override
	public void paymentTransactionSucceed(String transactionId) {
		System.out.println("Payment Success "+transactionId);

	}

	@Override
	public void paymentTransactionFailed(String errorMessage) {
		System.out.println("Payment failed "+errorMessage);

	}

}

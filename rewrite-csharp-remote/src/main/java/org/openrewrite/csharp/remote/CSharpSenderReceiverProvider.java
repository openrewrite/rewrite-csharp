package org.openrewrite.csharp.remote;

import com.google.auto.service.AutoService;
import org.openrewrite.csharp.tree.Cs;
import org.openrewrite.remote.Receiver;
import org.openrewrite.remote.Sender;
import org.openrewrite.remote.SenderReceiverProvider;

@SuppressWarnings("rawtypes")
@AutoService(SenderReceiverProvider.class)
public class CSharpSenderReceiverProvider implements SenderReceiverProvider<Cs> {
    @Override
    public Class<Cs> getType() {
        return Cs.class;
    }

    @Override
    public Sender<Cs> newSender() {
        return new CSharpSender();
    }

    @Override
    public Receiver<Cs> newReceiver() {
        return new CSharpReceiver();
    }
}

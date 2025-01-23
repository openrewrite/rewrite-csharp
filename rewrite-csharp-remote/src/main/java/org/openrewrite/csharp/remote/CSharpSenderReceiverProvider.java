/*
 * Copyright 2024 the original author or authors.
 * <p>
 * Licensed under the Moderne Source Available License (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * https://docs.moderne.io/licensing/moderne-source-available-license
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
